using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin_Demo
{
    public class OperationResult
    {
        public bool IsSuccess { get; set; } = false;

        public string Message { get; set; } = "Unknown";

        public int ErrorCode { get; set; } = 0;


        public OperationResult() { }

        public OperationResult(bool isSuccess) { this.IsSuccess = isSuccess; }

        public OperationResult(string msg) { this.Message = msg; }

        public OperationResult(bool isSuccess, string msg) : this(isSuccess) { this.Message = msg; }

        public OperationResult(bool isSuccess, string msg, int errorCode) : this(isSuccess, msg) { this.ErrorCode = errorCode; }



        public static OperationResult SuccessResult()
        {
            return new OperationResult(true, "Success");
        }

        public static OperationResult SuccessResult(string msg)
        {
            return new OperationResult(true, msg);
        }

        public static OperationResult FailResult()
        {
            return new OperationResult(false, "Fail");
        }

        public static OperationResult FailResult(string msg)
        {
            return new OperationResult(false, msg);
        }
        public static OperationResult FailResult(string msg, int errorCode)
        {
            return new OperationResult(false, msg, errorCode);
        }


        public static OperationResult<T> SuccessResult<T>(T value)
        {
            return new OperationResult<T>()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Success",
                Content = value
            };
        }

        public static OperationResult<T1, T2> SuccessResult<T1, T2>(T1 value1, T2 value2)
        {
            return new OperationResult<T1, T2>()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Success",
                Content1 = value1,
                Content2 = value2,
            };
        }

        public static OperationResult<T1, T2, T3> SuccessResult<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            return new OperationResult<T1, T2, T3>()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Success",
                Content1 = value1,
                Content2 = value2,
                Content3 = value3,
            };
        }

        public static OperationResult<T1, T2, T3, T4> SuccessResult<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            return new OperationResult<T1, T2, T3, T4>()
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "Success",
                Content1 = value1,
                Content2 = value2,
                Content3 = value3,
                Content4 = value4,
            };
        }

        public static OperationResult<T> FailResult<T>(T value,string msg)
        {
            return new OperationResult<T>()
            {
                Message = msg,
                Content = value
            };
        }

        public static OperationResult<T> FailResult<T>(T value, string msg,int errorCode)
        {
            return new OperationResult<T>()
            {
                ErrorCode = errorCode,
                Message = msg,
                Content= value
            };
        }



        public static OperationResult<T> FailResult<T>(OperationResult result)
        {
            return new OperationResult<T>()
            {
                ErrorCode = result.ErrorCode,
                Message = result.Message
            };
        }


        public static OperationResult<T1, T2> FailResult<T1, T2>(OperationResult result)
        {
            return new OperationResult<T1, T2>()
            {
                ErrorCode = result.ErrorCode,
                Message = result.Message
            };

        }


        public static OperationResult<T1, T2, T3> FailResult<T1, T2, T3>(OperationResult result)
        {
            return new OperationResult<T1, T2, T3>()
            {
                ErrorCode = result.ErrorCode,
                Message = result.Message
            };

        }


        public static OperationResult<T1, T2, T3, T4> FailResult<T1, T2, T3, T4>(OperationResult result)
        {
            return new OperationResult<T1, T2, T3, T4>()
            {
                ErrorCode = result.ErrorCode,
                Message = result.Message
            };

        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Content { get; set; }

        public OperationResult() : base()
        {

        }
    }

    public class OperationResult<T1, T2> : OperationResult
    {
        public T1 Content1 { get; set; }

        public T2 Content2 { get; set; }


        public OperationResult() : base()
        {

        }
    }

    public class OperationResult<T1, T2, T3> : OperationResult
    {
        public T1 Content1 { get; set; }

        public T2 Content2 { get; set; }

        public T3 Content3 { get; set; }

        public OperationResult() : base()
        {

        }
    }

    public class OperationResult<T1, T2, T3, T4> : OperationResult
    {
        public T1 Content1 { get; set; }

        public T2 Content2 { get; set; }

        public T3 Content3 { get; set; }

        public T4 Content4 { get; set; }


        public OperationResult() : base()
        {

        }
    }
}
