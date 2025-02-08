using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Net;
using System.Reflection;
using TaskMaster.Application.DTOs.Shared;
using TaskMaster.Application.DTOs.Tarefas;
using TaskMaster.Application.Interfaces;
using TaskMaster.Application.Services;
using TaskMaster.Domain.Entities;
using TaskMaster.Infrastructure.Data;
using TaskMaster.Presentation.Controllers;
using Xunit;

namespace TaskMaster.Tests.Tarefas
{
    public class TarefaControllerTests
    {
        Mock<ITarefaService> mockTarefaService;
        List<TarefaResponseDTO> mockTarefasResponse;

        public TarefaControllerTests()
        {
            mockTarefaService = new Mock<ITarefaService>();

            mockTarefasResponse = new List<TarefaResponseDTO>
            {
                new TarefaResponseDTO(
                    id: 1,
                    titulo: "Tarefa 1",
                    descricao: "Descrição 1",
                    dataCriacao: DateTime.Now,
                    dataConclusao: null,
                    status: Domain.Enums.Status.Pendente
                ),
                new TarefaResponseDTO(
                    id: 2,
                    titulo: "Tarefa 2",
                    descricao: "Descrição 2",
                    dataCriacao: DateTime.Now,
                    dataConclusao: DateTime.Now,
                    status: Domain.Enums.Status.Concluido
                )
            };

        }

        [Fact]
        public async Task GetAll_Retorna_Todas_Tarefas_Do_Banco_De_Dados()
        {
            //Arrange
            var mockResponse = new ResponseDTO<TarefaResponseDTO>(
                true,
                HttpStatusCode.OK,
                new List<string> { "Consulta realizada com sucesso" },
                mockTarefasResponse
            );

            //Act
            mockTarefaService
                .Setup(service => service.GetAllTarefasAsync())
                .ReturnsAsync(mockResponse);

            var controller = new TarefaController(mockTarefaService.Object);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnResponse = Assert.IsType<ResponseDTO<TarefaResponseDTO>>(okResult.Value);
            Assert.True(returnResponse.Success);
            Assert.Equal(HttpStatusCode.OK, returnResponse.StatusCode);
            Assert.Equal(2, returnResponse.Data.Count());
        }

        [Fact]
        public async Task GetById_Retorna_Tarefa()
        {
            //Arrange
            var mockResponse = new ResponseDTO<TarefaResponseDTO>(
                true,
                HttpStatusCode.OK,
                new List<string> { "Consulta realizada com sucesso" },
                new List<TarefaResponseDTO>()
                {
                    mockTarefasResponse[0]
                }
            );

            //Act
            mockTarefaService
                .Setup(service => service.GetTarefasByIdAsync(1))
                .ReturnsAsync(mockResponse);

            var controller = new TarefaController(mockTarefaService.Object);

            // Act
            var result = await controller.GetByIdAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnResponse = Assert.IsType<ResponseDTO<TarefaResponseDTO>>(okResult.Value);
            Assert.True(returnResponse.Success);
            Assert.Equal(HttpStatusCode.OK, returnResponse.StatusCode);
            Assert.Single(returnResponse.Data);
        }

        [Fact]
        public async Task GetById_Retorna_Erro_Quando_Id_Nao_Existe()
        {
            //Arrange
            var mockResponse = new ResponseDTO<TarefaResponseDTO>(
                false,
                HttpStatusCode.NotFound,
                new List<string> { "Nenhuma tarefa encontrada com o Id informado" }
            );

            //Act
            mockTarefaService
                .Setup(service => service.GetTarefasByIdAsync(5))
                .ReturnsAsync(mockResponse);

            var controller = new TarefaController(mockTarefaService.Object);

            // Act
            var result = await controller.GetByIdAsync(5);

            // Assert
            var okResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var returnResponse = Assert.IsType<ResponseDTO<TarefaResponseDTO>>(okResult.Value);
            Assert.False(returnResponse.Success);
            Assert.Equal(HttpStatusCode.NotFound, returnResponse.StatusCode);
            Assert.Empty(returnResponse.Data);
        }

        [Fact]
        public async Task Cria_Tarefa_E_Retorna_A_Tarefa()
        {
            //Arrange
            var tarefaInsertDTO = new TarefaInsertDTO("Tarefa 3", null);

            var mockResponse = new ResponseDTO<TarefaResponseDTO>(
                true,
                HttpStatusCode.OK,
                new List<string> { "Tarefa criada com sucesso" },
                new List<TarefaResponseDTO>()
                {
                    new TarefaResponseDTO(
                        id: 3,
                        titulo: tarefaInsertDTO.Titulo,
                        descricao: tarefaInsertDTO.Descricao,
                        dataCriacao: DateTime.Now,
                        dataConclusao: null,
                        status: Domain.Enums.Status.Pendente
                        )
                }
            );

            //Act
            mockTarefaService
                .Setup(service => service.AddTarefasAsync(tarefaInsertDTO))
                .ReturnsAsync(mockResponse);

            var controller = new TarefaController(mockTarefaService.Object);

            // Act
            var result = await controller.CreateTarefaAsync(tarefaInsertDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnResponse = Assert.IsType<ResponseDTO<TarefaResponseDTO>>(okResult.Value);
            Assert.True(returnResponse.Success);
            Assert.Equal(HttpStatusCode.OK, returnResponse.StatusCode);
            Assert.Single(returnResponse.Data);
        }

        [Fact]
        public async Task Retorna_Erro_Ao_Tentar_Criar_Tarefa_Sem_Titulo()
        {
            //Arrange
            var tarefaInsertDTO = new TarefaInsertDTO("", null);

            var mockResponse = new ResponseDTO<TarefaResponseDTO>(
                false,
                HttpStatusCode.BadRequest,
                new List<string> { "Erro ao criar tarefa." }
            );

            //Act
            mockTarefaService
                .Setup(service => service.AddTarefasAsync(tarefaInsertDTO))
                .ReturnsAsync(mockResponse);

            var controller = new TarefaController(mockTarefaService.Object);

            // Act
            var result = await controller.CreateTarefaAsync(tarefaInsertDTO);

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var returnResponse = Assert.IsType<ResponseDTO<TarefaResponseDTO>>(okResult.Value);
            Assert.False(returnResponse.Success);
            Assert.Equal(HttpStatusCode.BadRequest, returnResponse.StatusCode);
            Assert.Empty(returnResponse.Data);
        }

        [Fact]
        public async Task Atualiza_Tarefa_E_Retorna_A_Tarefa()
        {
            //Arrange
            var tarefaUpdateDTO = new TarefaUpdateDTO();
            tarefaUpdateDTO.Id = mockTarefasResponse[0].Id;
            tarefaUpdateDTO.Status = Domain.Enums.Status.Concluido;
            tarefaUpdateDTO.Descricao = "Descrição Atualizada";
            tarefaUpdateDTO.Titulo = mockTarefasResponse[0].Titulo;
            tarefaUpdateDTO.DataConclusao = DateTime.Now;

            var mockResponse = new ResponseDTO<TarefaResponseDTO>(
                true,
                HttpStatusCode.OK,
                new List<string> { "Tarefa atualizada com sucesso" },
                new List<TarefaResponseDTO>()
                {
                    new TarefaResponseDTO(
                        id: tarefaUpdateDTO.Id,
                        titulo: tarefaUpdateDTO.Titulo,
                        descricao: tarefaUpdateDTO.Descricao,
                        dataCriacao: mockTarefasResponse[0].DataCriacao,
                        dataConclusao: tarefaUpdateDTO.DataConclusao,
                        status: Domain.Enums.Status.Concluido
                        )
                }
            );

            //Act
            mockTarefaService
                .Setup(service => service.UpdateTarefasAsync(tarefaUpdateDTO))
                .ReturnsAsync(mockResponse);

            var controller = new TarefaController(mockTarefaService.Object);

            // Act
            var result = await controller.UpdateTarefaAsync(tarefaUpdateDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnResponse = Assert.IsType<ResponseDTO<TarefaResponseDTO>>(okResult.Value);
            Assert.True(returnResponse.Success);
            Assert.Equal(HttpStatusCode.OK, returnResponse.StatusCode);
            Assert.Single(returnResponse.Data);
        }

        [Fact]
        public async Task Retorna_Erro_Ao_Tentar_Atualizar_Tarefa_Inexistente()
        {
            //Arrange
            var tarefaUpdateDTO = new TarefaUpdateDTO();
            tarefaUpdateDTO.Id = 50000;
            tarefaUpdateDTO.Status = Domain.Enums.Status.Concluido;
            tarefaUpdateDTO.Descricao = "Descrição Atualizada";
            tarefaUpdateDTO.Titulo = mockTarefasResponse[0].Titulo;
            tarefaUpdateDTO.DataConclusao = DateTime.Now;

            var mockResponse = new ResponseDTO<TarefaResponseDTO>(
                false,
                HttpStatusCode.NotFound,
                new List<string> { "Nenhuma tarefa encontrada com o Id informado" }
            );

            //Act
            mockTarefaService
                .Setup(service => service.UpdateTarefasAsync(tarefaUpdateDTO))
                .ReturnsAsync(mockResponse);

            var controller = new TarefaController(mockTarefaService.Object);

            // Act
            var result = await controller.UpdateTarefaAsync(tarefaUpdateDTO);

            // Assert
            var okResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var returnResponse = Assert.IsType<ResponseDTO<TarefaResponseDTO>>(okResult.Value);
            Assert.False(returnResponse.Success);
            Assert.Equal(HttpStatusCode.NotFound, returnResponse.StatusCode);
            Assert.Empty(returnResponse.Data);
        }

        [Fact]
        public async Task Retorna_Erro_Ao_Tentar_Atualizar_Tarefa_Sem_Informar_Titulo()
        {
            //Arrange
            var tarefaUpdateDTO = new TarefaUpdateDTO();
            tarefaUpdateDTO.Id = mockTarefasResponse[0].Id;
            tarefaUpdateDTO.Status = Domain.Enums.Status.Concluido;
            tarefaUpdateDTO.Descricao = "Descrição Atualizada";
            tarefaUpdateDTO.Titulo = "";
            tarefaUpdateDTO.DataConclusao = DateTime.Now;

            var mockResponse = new ResponseDTO<TarefaResponseDTO>(
                false,
                HttpStatusCode.BadRequest,
                new List<string> { "Nenhuma tarefa encontrada com o Id informado" }
            );

            //Act
            mockTarefaService
                .Setup(service => service.UpdateTarefasAsync(tarefaUpdateDTO))
                .ReturnsAsync(mockResponse);

            var controller = new TarefaController(mockTarefaService.Object);

            // Act
            var result = await controller.UpdateTarefaAsync(tarefaUpdateDTO);

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var returnResponse = Assert.IsType<ResponseDTO<TarefaResponseDTO>>(okResult.Value);
            Assert.False(returnResponse.Success);
            Assert.Equal(HttpStatusCode.BadRequest, returnResponse.StatusCode);
            Assert.Empty(returnResponse.Data);
        }

        [Fact]
        public async Task Deleta_Tarefa_E_Retorna_Lista_Vazia()
        {
            //Arrange
            var mockResponse = new ResponseDTO<TarefaResponseDTO>(
                true,
                HttpStatusCode.OK,
                new List<string> { "Tarefa excluida com sucesso" },
                new List<TarefaResponseDTO>()
            );

            //Act
            mockTarefaService
                .Setup(service => service.DeleteTarefasAsync(1))
                .ReturnsAsync(mockResponse);

            var controller = new TarefaController(mockTarefaService.Object);

            // Act
            var result = await controller.DeleteTarefaAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnResponse = Assert.IsType<ResponseDTO<TarefaResponseDTO>>(okResult.Value);
            Assert.True(returnResponse.Success);
            Assert.Equal(HttpStatusCode.OK, returnResponse.StatusCode);
            Assert.Empty(returnResponse.Data);
        }

        [Fact]
        public async Task Retorna_Erro_Ao_Tentar_Excluir_Tarefa_Inexistente()
        {
            //Arrange
            var mockResponse = new ResponseDTO<TarefaResponseDTO>(
                false,
                HttpStatusCode.NotFound,
                new List<string> { "Nenhuma tarefa encontrada com o Id informado" }
            );

            //Act
            mockTarefaService
                .Setup(service => service.DeleteTarefasAsync(5000))
                .ReturnsAsync(mockResponse);

            var controller = new TarefaController(mockTarefaService.Object);

            // Act
            var result = await controller.DeleteTarefaAsync(5000);

            // Assert
            var okResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var returnResponse = Assert.IsType<ResponseDTO<TarefaResponseDTO>>(okResult.Value);
            Assert.False(returnResponse.Success);
            Assert.Equal(HttpStatusCode.NotFound, returnResponse.StatusCode);
            Assert.Empty(returnResponse.Data);
        }
    }
}