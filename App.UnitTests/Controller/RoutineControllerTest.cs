using App.BLL.Services;
using App.DAL.Interfaces;
using App.DAL.Models;
using App.DAL.Repository;
using App.PL.Controllers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.UnitTests.Controller
{
    public class RoutineControllerTest
    {
        private RoutineController _routineController;
        private IRoutineRepository _routineRepository;
        private IPhotoService _photoService;
        private IHttpContextAccessor _httpContextAccessor;
        public RoutineControllerTest()
        {
            //Dependencies
            _routineRepository = A.Fake<IRoutineRepository>();
            _photoService = A.Fake<IPhotoService>();
            _httpContextAccessor = A.Fake<HttpContextAccessor>();

            //SUT
            _routineController = new RoutineController(_routineRepository, _photoService, _httpContextAccessor);
        }

        [Fact]
        public void RoutineController_Index_ReturnsSuccess()
        {
            //Arrange - What do i need to bring in?
            var routines = A.Fake<IEnumerable<Routine>>();
            A.CallTo(() => _routineRepository.GetAll()).Returns(routines);
            //Act
            var result = _routineController.Index();
            //Assert - Object check actions
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void RoutineController_Detail_ReturnsSuccess()
        {
            //Arrange
            var id = 1;
            var routine = A.Fake<Routine>();
            A.CallTo(() => _routineRepository.GetByIdAsync(id)).Returns(routine);
            //Act
            var result = _routineController.Detail(id);
            //Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}
