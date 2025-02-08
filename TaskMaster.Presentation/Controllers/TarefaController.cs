using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TaskMaster.Application.DTOs.Shared;
using TaskMaster.Application.DTOs.Tarefas;
using TaskMaster.Application.Interfaces;
using TaskMaster.Application.Services;
using TaskMaster.Domain.Entities;

namespace TaskMaster.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;

        public TarefaController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDTO<TarefaResponseDTO>>> GetAllAsync()
        {
            try{
                var response = await _tarefaService.GetAllTarefasAsync();
                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                return Problem($"Ocorreu um erro ao buscar a tarefa. Detalhes: {ex.Message}.", "GetByIdAsync", statusCode: 500, "Erro ao buscar tarefa.");    
            }
            catch (Exception ex)
            {
                return Problem($"Ocorreu uma exceção sem tratamento. Detalhes: {ex.Message}.", "GetByIdAsync", statusCode: 500, "Erro ao buscar tarefa.");    
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseDTO<TarefaResponseDTO>>> GetByIdAsync(int id)
        {
            try{
                var response = await _tarefaService.GetTarefasByIdAsync(id);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem($"Nenhuma tarefa encontrada com o Id informado. Detalhes: {ex.Message}.", "GetByIdAsync", statusCode: 404, "Tarefa não encontrada.");    
            }
            catch (ApplicationException ex)
            {
                return Problem($"Ocorreu um erro ao buscar a tarefa. Detalhes: {ex.Message}.", "GetByIdAsync", statusCode: 500, "Erro ao buscar tarefa.");    
            }
            catch (Exception ex)
            {
                return Problem($"Ocorreu uma exceção sem tratamento. Detalhes: {ex.Message}.", "GetByIdAsync", statusCode: 500, "Erro ao buscar tarefa.");    
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTarefaAsync([FromBody] TarefaInsertDTO tarefa)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                            .Where(y=>y.Count>0)
                            .ToList().SelectMany(x=>x).Select(x=>x.ErrorMessage).ToList();
                
                return BadRequest(
                    new ResponseDTO<TarefaResponseDTO>(
                        false,
                        HttpStatusCode.BadRequest,
                        errors
                    )
                );
            }

            try{
                var response = await _tarefaService.AddTarefasAsync(tarefa);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem($"Nenhuma tarefa encontrada com o Id informado. Detalhes: {ex.Message}.", "InsertTarefaAsync", statusCode: 404, "Tarefa não encontrada.");    
            }
            catch (ValidationException ex)
            {
                return Problem($"Ocorreu um erro de validação ao inserir a tarefa. Detalhes: {ex.Message}.", "InsertTarefaAsync", statusCode: 400, "Erro ao inserir tarefa.");    
            }
            catch (ApplicationException ex)
            {
                return Problem($"Ocorreu um erro ao atualizar a tarefa. Detalhes: {ex.Message}.", "InsertTarefaAsync", statusCode: 500, "Erro ao inserir tarefa.");    
            }
            catch (Exception ex)
            {
                return Problem($"Ocorreu uma exceção sem tratamento. Detalhes: {ex.Message}.", "InsertTarefaAsync", statusCode: 500, "Erro ao inserir tarefa.");    
            }
        }

        [HttpPut]
        public async Task<ActionResult<ResponseDTO<TarefaResponseDTO>>> UpdateTarefaAsync([FromBody] TarefaUpdateDTO tarefaUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                            .Where(y=>y.Count>0)
                            .ToList().SelectMany(x=>x).Select(x=>x.ErrorMessage).ToList();
                return BadRequest(new ResponseDTO<TarefaResponseDTO>(
                    false,
                    HttpStatusCode.BadRequest,
                    errors
                ));
            }

            try{
                var response = await _tarefaService.UpdateTarefasAsync(tarefaUpdateDTO);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem($"Nenhuma tarefa encontrada com o Id informado. Detalhes: {ex.Message}.", "UpdateTarefaAsync", statusCode: 404, "Tarefa não encontrada.");    
            }
            catch (ValidationException ex)
            {
                return Problem($"Ocorreu um erro de validação ao atualizar a tarefa. Detalhes: {ex.Message}.", "UpdateTarefaAsync", statusCode: 400, "Erro ao atualizar tarefa.");    
            }
            catch (ApplicationException ex)
            {
                return Problem($"Ocorreu um erro ao atualizar a tarefa. Detalhes: {ex.Message}.", "UpdateTarefaAsync", statusCode: 500, "Erro ao atualizar tarefa.");    
            }
            catch (Exception ex)
            {
                return Problem($"Ocorreu uma exceção sem tratamento. Detalhes: {ex.Message}.", "UpdateTarefaAsync", statusCode: 500, "Tarefa não encontrada.");    
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseDTO<TarefaResponseDTO>>> DeleteTarefaAsync(int id)
        {
            try{
                var response = await _tarefaService.DeleteTarefasAsync(id);

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return Problem($"Nenhuma tarefa encontrada com o Id informado. Detalhes: {ex.Message}.", "DeleteTarefasAsync", statusCode: 404, "Erro ao excluir tarefa.");    
            }
            catch (ApplicationException ex)
            {
                return Problem($"Ocorreu um erro ao atualizar a tarefa. Detalhes: {ex.Message}.", "DeleteTarefasAsync", statusCode: 500, "Erro ao excluir tarefa.");    
            }
            catch (Exception ex)
            {
                return Problem($"Ocorreu uma exceção sem tratamento. Detalhes: {ex.Message}.", "DeleteTarefasAsync", statusCode: 500, "Erro ao excluir tarefa.");    
            }
        }
    }
}