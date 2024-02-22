using System;
using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model
{
    [Serializable]
    public class Request<T> : IRequest<T>
    {
        [Required]
        public T Data { get; set; }

    }
}