using App.BLL.DataEnums;
using App.DAL.Context;
using App.DAL.Models;
using App.DAL.Repository;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.UnitTests.Repository
{
    public class ClubRepositoryTest
    {
        private async Task<AppDbContext> GetDbContext() { 
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Clubs.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Clubs.Add(
                        new Club()
                        {
                            Title = $"Running Club 1",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = $"This is the description of the first club",
                            ClubCategory = ClubCategory.Running,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Michigan",
                                State = "NC"
                            }
                        });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }
        [Fact]
        public async void ClubRepository_Add_ReturnsBool()
        {
            //Arrange
            var club = new Club()
            {
                Title = $"Running Club 1",
                Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                Description = $"This is the description of the first club",
                ClubCategory = ClubCategory.Running,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Michigan",
                    State = "NC"
                }
            };
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            var result = clubRepository.Add(club);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async void ClubRepository_GetByIdAsync_ReturnsClub()
        {
            //Arrange
            var id = 1;
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            var result = clubRepository.GetByIdAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Club>>();
        }

        [Fact]
        public async void ClubRepository_GetAll_ReturnsList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            var result = await clubRepository.GetAll();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Club>>();
        }

        [Fact]
        public async void ClubRepository_SuccessfulDelete_ReturnsTrue()
        {
            //Arrange
            var club = new Club()
            {
                Title = $"Running Club 1",
                Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                Description = $"This is the description of the first club",
                ClubCategory = ClubCategory.Running,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Michigan",
                    State = "NC"
                }
            };
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            clubRepository.Add(club);
            var result = clubRepository.Delete(club);
            var count = await clubRepository.GetCountAsync();

            //Assert
            result.Should().BeTrue();
            count.Should().Be(0);
        }

        [Fact]
        public async void ClubRepository_GetCountAsync_ReturnsInt()
        {
            //Arrange
            var club = new Club()
            {
                Title = $"Running Club 1",
                Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                Description = $"This is the description of the first club",
                ClubCategory = ClubCategory.Running,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Michigan",
                    State = "NC"
                }
            };
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            clubRepository.Add(club);
            var result = await clubRepository.GetCountAsync();

            //Assert
            result.Should().Be(1);
        }
    }
}
