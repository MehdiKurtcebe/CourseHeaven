using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Web.Services;

public class ServiceResult
{
    public ProblemDetails? Fail { get; set; }

    [JsonIgnore] public bool IsSuccess => Fail is null;
    [JsonIgnore] public bool IsFail => !IsSuccess;

    public static ServiceResult Success()
    {
        return new ServiceResult();
    }

    public static ServiceResult Error(ProblemDetails problemDetails)
    {
        return new ServiceResult
        {
            Fail = problemDetails
        };
    }

    public static ServiceResult Error(string title, string description)
    {
        return new ServiceResult
        {
            Fail = new ProblemDetails
            {
                Title = title,
                Detail = description
            }
        };
    }

    public static ServiceResult Error(string title)
    {
        return new ServiceResult
        {
            Fail = new ProblemDetails
            {
                Title = title
            }
        };
    }
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; set; }

    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T>
        {
            Data = data
        };
    }

    public new static ServiceResult<T> Error(ProblemDetails problemDetails)
    {
        return new ServiceResult<T>
        {
            Fail = problemDetails
        };
    }

    public new static ServiceResult<T> Error(string title, string description)
    {
        return new ServiceResult<T>
        {
            Fail = new ProblemDetails
            {
                Title = title,
                Detail = description
            }
        };
    }

    public new static ServiceResult<T> Error(string title)
    {
        return new ServiceResult<T>
        {
            Fail = new ProblemDetails
            {
                Title = title
            }
        };
    }
}