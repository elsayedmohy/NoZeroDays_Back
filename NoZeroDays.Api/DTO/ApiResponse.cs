using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace NoZeroDays.Api.DTO;

public sealed class ApiResponse<T>
{
    public T? Data { get; init; }
    public string? Message { get; init; }

    public static ApiResponse<T> Ok(T data, string? message = null) =>
        new()
        {
            Data = data,
            Message = message
        };

    public static ApiResponse<T> Fail(string message) =>
        new()
        {
            Message = message
        };
}
