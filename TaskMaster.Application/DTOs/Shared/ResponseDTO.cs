using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TaskMaster.Application.DTOs.Shared
{
    public class ResponseDTO<T>
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Message { get; set; } = new List<string>();
        public List<T> Data { get; set; } = new List<T>();

        public ResponseDTO() { }

        public ResponseDTO(IEnumerable<T> data) { Data = data.ToList(); }

        public ResponseDTO(bool success, HttpStatusCode statusCode, List<string> message, IEnumerable<T> data) 
        { 
            Success = success; 
            StatusCode = statusCode;
            Message = message; 
            Data = data.ToList(); 
        }

        public ResponseDTO(bool success, HttpStatusCode statusCode, List<string> message)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
        }

    }
}