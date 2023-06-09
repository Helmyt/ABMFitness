﻿using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using App.PL.Controllers;
using App.DAL.Interfaces;
using App.DAL.Models;
using App.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.UnitTests.Controller
{
    public class ClubControllerTest
    {
        private ClubController _clubController;
        private IClubRepository _clubRepository;
        private IPhotoService _photoService;
        private IHttpContextAccessor _httpContextAccessor;
        public ClubControllerTest()
        {
            //Dependencies
            _clubRepository = A.Fake<IClubRepository>();
            _photoService = A.Fake<IPhotoService>();
            _httpContextAccessor = A.Fake<HttpContextAccessor>();

            //SUT
            _clubController = new ClubController(_clubRepository, _photoService);
        }

        [Fact]
        public void ClubController_Index_ReturnsSuccess()
        {
            //Arrange - What do i need to bring in?
            var clubs = A.Fake<IEnumerable<Club>>();
            A.CallTo(() => _clubRepository.GetAll()).Returns(clubs);
            //Act
            var result = _clubController.Index();
            //Assert - Object check actions
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void ClubController_Detail_ReturnsSuccess()
        {
            //Arrange
            var id = 1;
            var club = A.Fake<Club>();
            A.CallTo(() => _clubRepository.GetByIdAsync(id)).Returns(club);
            //Act
            var result = _clubController.Detail(id);
            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
        [Fact]
        public void ClubController_Edit_ReturnsSuccess()
        {
            //Arrange
            var id = 1;
            var club = A.Fake<Club>();
            A.CallTo(() => _clubRepository.GetByIdAsync(id)).Returns(club);
            //Act
            var result = _clubController.Edit(id);
            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
        [Fact]
        public void ClubController_Delete_ReturnsSuccess()
        {
            //Arrange
            var id = 1;
            var club = A.Fake<Club>();
            A.CallTo(() => _clubRepository.GetByIdAsync(id)).Returns(club);
            //Act
            var result = _clubController.Delete(id);
            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
