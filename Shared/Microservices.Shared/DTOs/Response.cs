using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Microservices.Shared.DTOs
{
    public class Response<T>
    {
        public T Data { get; private set; } //dısardan nesne ornegni degistiremeyecek

        [JsonIgnore]
        public int StatusCode { get; private set; }  //bu properryden fayfalancaz ama respnse icinde olmasına grek yok yansımasın postmandde gozuken status code bu biseye vrunca

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }  //client jsonignore ile karıslasmayacak kodda kullancaz

        public List<string> Errors { get; set; }

        //Static Factory Methods
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T>
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccessful = false };
        }
    }
}
