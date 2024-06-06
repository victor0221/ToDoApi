using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.Controllers;
using ToDoApi.Interfaces;
using ToDoApi.Models;
using ToDoApi.Dtos.toDo;
using System;
using Microsoft.AspNetCore.Http;

namespace UnitTest
{
    public class ToDoControllerTests
    {
        private readonly Mock<IToDoRepository> _mockRepo;
        private readonly ToDoController _controller;

        public ToDoControllerTests()
        {
            _mockRepo = new Mock<IToDoRepository>();
            _controller = new ToDoController(_mockRepo.Object);
        }

        [Fact]
        public async Task ToDoGetAll_ReturnsOkResult()
        {
            // Arrange
            var mockToDoList = new List<ToDo> { new ToDo { Id = 1, Name = "Test ToDo" } };
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(mockToDoList);

            // Act
            var result = await _controller.ToDoGetAll(1, 10, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnValue = Assert.IsType<List<ToDo>>(okResult.Value);
            Assert.Equal(mockToDoList.Count, returnValue.Count);
        }

        [Fact]
        public async Task ToDoGetOne_ReturnsOkResult()
        {
            // Arrange
            var mockToDo = new ToDo { Id = 1, Name = "Test ToDo" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockToDo);

            // Act
            var result = await _controller.ToDoGetOne(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnValue = Assert.IsType<ToDo>(okResult.Value);
            Assert.Equal(mockToDo.Id, returnValue.Id);
        }

        [Fact]
        public async Task ToDoUpdateOne_ReturnsOkResult()
        {
            // Arrange
            var mockToDoDto = new ToDoDto { Name = "Updated ToDo" };
            var mockToDo = new ToDo { Id = 1, Name = "Updated ToDo" };
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<ToDoDto>())).ReturnsAsync(mockToDo);

            // Act
            var result = await _controller.ToDoUpdateOne(1, mockToDoDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnValue = Assert.IsType<ToDo>(okResult.Value);
            Assert.Equal(mockToDo.Name, returnValue.Name);
        }

        [Fact]
        public async Task ToDoDeleteOne_ReturnsOkResult()
        {
            // Arrange
            var mockToDo = new ToDo { Id = 1, Name = "Deleted ToDo" };
            _mockRepo.Setup(repo => repo.Delete(It.IsAny<int>())).ReturnsAsync(mockToDo);

            // Act
            var result = await _controller.ToDoDeleteOne(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnValue = Assert.IsType<ToDo>(okResult.Value);
            Assert.Equal(mockToDo.Name, returnValue.Name);
        }

        [Fact]
        public async Task ToDoCreateOne_ReturnsOkResult()
        {
            // Arrange
            var mockToDoDto = new ToDoDto { Name = "New ToDo" };
            var mockToDo = new ToDo { Id = 1, Name = "New ToDo" };
            _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<ToDoDto>())).ReturnsAsync(mockToDo);

            // Act
            var result = await _controller.ToDoCreateOne(mockToDoDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnValue = Assert.IsType<ToDo>(okResult.Value);
            Assert.Equal(mockToDo.Name, returnValue.Name);
        }

        [Fact]
        public async Task ToDoGetAll_ReturnsInternalServerError()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.ToDoGetAll(1, 10, null, null, null);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task ToDoGetOne_ReturnsInternalServerError()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.ToDoGetOne(1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task ToDoUpdateOne_ReturnsInternalServerError()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<ToDoDto>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.ToDoUpdateOne(1, new ToDoDto { Name = "Test" });

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task ToDoDeleteOne_ReturnsInternalServerError()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Delete(It.IsAny<int>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.ToDoDeleteOne(1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task ToDoCreateOne_ReturnsInternalServerError()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<ToDoDto>())).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.ToDoCreateOne(new ToDoDto { Name = "Test" });

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}
