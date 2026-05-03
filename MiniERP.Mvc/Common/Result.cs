using System;

namespace MiniERP.Mvc.Common;

public enum ErrorCode
{
    None = 0,
    BadRequest = 400,
    NotFound = 404,
    Conflict = 409,
    InternalServerError = 500
}

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? ErrorMessage { get; }
    public ErrorCode ErrorCode { get; }

    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, T? data, string? errorMessage, ErrorCode errorCode)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public static Result<T> Success(T value) => new(true, value, null, ErrorCode.None);
    public static Result<T> Failure(string message, ErrorCode code) => new(false, default, message, code);
}

public class Result : Result<bool>
{
    protected Result(bool isSuccess, string? errorMessage, ErrorCode errorCode) : base(isSuccess, isSuccess, errorMessage, errorCode) { }

    public static Result Success() => new(true, null, ErrorCode.None);
    public static new Result Failure(string errorMessage, ErrorCode errorCode) => new(false, errorMessage, errorCode);
}