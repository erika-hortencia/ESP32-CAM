using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace backend.Data
{
    /// <summary>
    /// Maps relational table 'registers'.
    /// </summary>
    [Table("tbl_registers"), DataContract]
    public class Register
    {

        [Column("int_register_id"), DataMember]
        [Key]
        public Int32 id { get; set; }

        [Column("chr_register_name"), DataMember]
        public string name { get; set; }

        [Column("blob_register_image"), DataMember]
        public byte[] image { get; set; }

        [Column("int_register_type"), DataMember]
        public int type { get; set; }

        [Column("dte_register_creation_time"), DataMember]
        public DateTime creation_time { get; set; }


        public Register()
        {
            this.type = 1;
            this.creation_time = DateTime.UtcNow.ToUniversalTime().AddHours(-3);
        }

    }
}