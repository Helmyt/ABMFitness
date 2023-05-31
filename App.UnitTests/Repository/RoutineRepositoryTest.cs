using App.BLL.DataEnums;
using App.DAL.Context;
using App.DAL.Interfaces;
using App.DAL.Models;
using App.DAL.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.UnitTests.Repository
{
    public class RoutineRepositoryTest
    {
        private async Task<AppDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Routines.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Routines.Add(
                        new Routine()
                        {
                            Title = "Running Race 1",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first race",
                            RoutineCategory = RoutineCategory.Marathon,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
        [Fact]
        public async void RoutineRepository_Add_ReturnsBool()
        {
            //Arrange
            var routine = new Routine()
            {
                Title = "Running Race 1",
                Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                Description = "This is the description of the first race",
                RoutineCategory = RoutineCategory.Marathon,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Charlotte",
                    State = "NC"
                }
            };
            var dbContext = await GetDbContext();
            var routineRepository = new RoutineRepository(dbContext);

            //Act
            var result = routineRepository.Add(routine);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async void ClubRepository_GetByIdAsync_ReturnsClub()
        {
            //Arrange
            var id = 1;
            var dbContext = await GetDbContext();
            var routineRepository = new RoutineRepository(dbContext);

            //Act
            var result = routineRepository.GetByIdAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Routine>>();
        }

        [Fact]
        public async void ClubRepository_GetAll_ReturnsList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var routineRepository = new RoutineRepository(dbContext);

            //Act
            var result = await routineRepository.GetAll();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Routine>>();
        }

        [Fact]
        public async void ClubRepository_SuccessfulDelete_ReturnsTrue()
        {
            //Arrange
            var routine = new Routine()
            {
                Title = "Running Race 1",
                Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                Description = "This is the description of the first race",
                RoutineCategory = RoutineCategory.Marathon,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Charlotte",
                    State = "NC"
                }
            };
            var dbContext = await GetDbContext();
            var routineRepository = new RoutineRepository(dbContext);

            //Act
            routineRepository.Add(routine);
            var result = routineRepository.Delete(routine);
            var count = await routineRepository.GetCountAsync();

            //Assert
            result.Should().BeTrue();
            count.Should().Be(0);
        }

        [Fact]
        public async void ClubRepository_GetCountAsync_ReturnsInt()
        {
            //Arrange
            var routine = new Routine()
            {
                Title = "Running Race 1",
                Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                Description = "This is the description of the first race",
                RoutineCategory = RoutineCategory.Marathon,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Charlotte",
                    State = "NC"
                }
            };
            var dbContext = await GetDbContext();
            var routineRepository = new RoutineRepository(dbContext);

            //Act
            routineRepository.Add(routine);
            var result = await routineRepository.GetCountAsync();

            //Assert
            result.Should().Be(1);
        }
    }

}
