namespace BAGeocoding.Api.Models
{
    public interface IResult<T>
    {
        // Mã trả về thành công hoặc lỗi.
        // Nếu trả về mã code "ok" nghĩa là thành công.
        // Ngược lại, nếu lỗi thì trả về mã lỗi tương ứng.
        string? Code { get; set; }

        // Nội dung của mã lỗi(nếu có).
        string? Message { get; set; }

        // Danh sách "place" được tìm thấy, nếu lỗi thì result là "null".
        T? Data { get; set; }
    }

    public class Result<T> : IResult<T>
    {
        public T? Data { get; set; } = default(T?);
        public string? Code { get; set; } = null;
        public string? Message { get; set; } = null;
        public string? ProcessState { get; set; } = null;

        public static Result<T> Success(T data,string code = "", string message = "", string processState = "")
        {
            return new Result<T>()
            {
                Code = code,
                Message = message,
                Data = data,
                ProcessState = processState,
            };
        }

        public static Result<T> Error(string code,string message = "", string processState = "") => new Result<T>()
        {
            Code = code,
            Message = message,
            ProcessState = processState
        };
    }
}
