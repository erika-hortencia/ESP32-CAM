using System;
using System.Collections.Generic;

namespace backend.Dto
{
    /// <summary>
    /// Main infos data.
    /// </summary>
    public class RegisterInfosDto
    {
        public int? type { get; set; }
        public string name { get; set; }
        public byte[] image { get; set; }
        public DateTime? creation_time { get; set; }
    }

    /// <summary>
    /// Data that can be updated.
    /// </summary>
    public class RegisterUpdateDto
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}