﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raptor.Core.Helpers;
using Raptor.Data.Models.Users;
using Raptor.Services.Helpers;
using Raptor.Services.Logging;
using Raptor.Services.Users;
using Raptor.Web.Areas.Admin.ViewModels;
using Raptor.Web.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Raptor.Data.Models.Logging;

namespace Raptor.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("admin")]
    public class AccountController : Controller
    {
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerActivityService _activityService;

        public AccountController(ILogService logService, IUserService userService, IWorkContext workContext, ICustomerActivityService activityService) {
            _logService = logService;
            _userService = userService;
            _workContext = workContext;
            _activityService = activityService;
        }

        [HttpGet]
        public IActionResult Settings() {
            var emailClaim = User.Claims.FirstOrDefault(c => c.ValueType == ClaimTypes.Email);

            if (emailClaim == null) return null;

            var currentUser = _userService.GetUserByEmail(emailClaim.Value);
            var userViewModel = AutoMapper.Mapper.Map<Person, UserViewModel>(currentUser);

            userViewModel.FacebookUrl = currentUser.SocialProfile.FacebookUrl;
            userViewModel.TwitterUrl = currentUser.SocialProfile.TwitterUrl;
            userViewModel.YoutubeUrl = currentUser.SocialProfile.YoutubeUrl;
            userViewModel.InstagramUrl = currentUser.SocialProfile.InstagramUrl;

            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Settings(UserViewModel model, IFormFile avatarSelect) {
            if (!ModelState.IsValid) return View(model);

            try {
                var emailClaim = User.Claims.FirstOrDefault(c => c.ValueType == ClaimTypes.Email);
                if (emailClaim == null) return null;

                var currentUser = _userService.GetUserByEmail(emailClaim.Value);

                // Check if it is a valid email address
                if (!CommonHelper.IsValidEmail(model.EmailAddress)) {
                    ModelState.AddModelError("", "Email Address is not valid.");
                    return View(model);
                }

                // Check if the user updated their email address
                if (model.EmailAddress != currentUser.EmailAddress) {
                    var emailExists = _userService.CheckIfUserExistsByEmail(model.EmailAddress);
                    if (emailExists) {
                        ModelState.AddModelError("", "A user with a similar email address already exists.");
                        return View(model);
                    }
                }

                // Check if the user updated their username
                if (model.Username != currentUser.Username) {
                    var userExists = _userService.CheckIfUserExistsByUsername(model.Username);
                    if (userExists) {
                        ModelState.AddModelError("", "Username not available.");
                        return View(model);
                    }
                }

                // Uploaded avatar if any
                if (avatarSelect != null) {
                    currentUser.Avatar = $"data:{avatarSelect.ContentType};base64,{Convert.ToBase64String(CommonHelper.GetByteArrayFromImage(avatarSelect))}";
                    model.Avatar = currentUser.Avatar;
                }

                currentUser.FirstName = model.FirstName;
                currentUser.MiddleName = model.MiddleName;
                currentUser.LastName = model.LastName;
                currentUser.DisplayName = $"{model.FirstName} {model.LastName}";
                currentUser.EmailAddress = model.EmailAddress;
                currentUser.Username = model.Username;
                currentUser.About = model.About;
                currentUser.Website = model.Website;
                currentUser.DisplayName = !string.IsNullOrEmpty(model.MiddleName) ? $"{model.FirstName} {model.MiddleName} {model.LastName}" : $"{model.FirstName} {model.LastName}";

                currentUser.SocialProfile.FacebookUrl = model.FacebookUrl;
                currentUser.SocialProfile.InstagramUrl = model.InstagramUrl;
                currentUser.SocialProfile.YoutubeUrl = model.YoutubeUrl;
                currentUser.SocialProfile.TwitterUrl = model.TwitterUrl;

                _userService.UpdateUser(currentUser);
                _activityService.InsertActivity(_workContext.CurrentUser, ActivityLogDefaults.UpdateProfile, "Updated profile information.");

                
                _workContext.CurrentUser = currentUser;

                ViewBag.Status = "OK";
                ViewBag.Message = "Your changes have been saved successfully.";
            }
            catch (Exception ex) {
                _logService.InsertLog(LogLevel.Error, "Unable to update user account settings.", ex.ToString());
                ModelState.AddModelError("", ex.ToString());
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ResetPasswordViewModel model) {
            if (!ModelState.IsValid) return View(model);

            if (model.Password != model.RepeatPassword) {
                ModelState.AddModelError("", "The passwords do not match.");
                return View(model);
            }

            if (model.Password.Length < 8) {
                ModelState.AddModelError("", "Password must be at least 8 characters long.");
                return View(model);
            }

            _userService.UpdatePassword(_workContext.CurrentUser.EmailAddress, model.Password);
            _activityService.InsertActivity(_workContext.CurrentUser, ActivityLogDefaults.UpdatePassword, "Updated password.");

            ViewBag.Status = "OK";
            ViewBag.Message = "Your password has been changed successfully";

            return View(model);
        }

        [HttpGet]
        public IActionResult Activity() {
            var activityLog = _activityService.GetActivityForUser(_workContext.CurrentUser.BusinessEntityId);
            _activityService.InsertActivity(_workContext.CurrentUser, ActivityLogDefaults.ViewLogs, "Viewed activity logs");
            return View(activityLog);
        }
    }
}