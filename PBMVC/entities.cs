using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PicBook
{

    public abstract class Entity
    {
        [Key]
        public Guid ID { get; set; }

        public Entity()
        {
            ID = Guid.NewGuid();
            IsActive = true;
            Created = DateTime.Now;
        }

        public bool IsActive { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

    }


    [Table("image")]
    public class Image : Entity
    {
        [Column("title")]
        public String Title { get; set; }

        [Column("file_path")]
        public String FilePath { get; set; }

        [Column("file_tags")]
        [InverseProperty("id")]
        public ICollection<ImageTag> Tags { get; set; }

        [Column("albumid")]
        public Album album { get; set; }
    }

    public class ImageTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Column("tag_name")]
        public string tagname { get; set; }

        public Image image { get; set; }


    }

    public enum Visibility
    {
        Private, Public, Shared
    }

    [Table("album")]
    public class Album : Entity
    {
        [InverseProperty("albumid")]
        public ICollection<Image> Images { get; set; }

        [Column("title")]
        public String Title { get; set; }

        [Column("visibility")]
        public Visibility Visibility { get; set; }

        /* Empty if Visibility is Private or Public */
        [InverseProperty("album")]
        public ICollection<Sharerelation> SharedWith { get; set; }
        

        public User Owner { get; set; }


    }

    [Table("Sharerelation")]
    public class Sharerelation
    {
        [Column("userid")]
        public User user { get; set; }

        [Column("albumid")]
        public Album album { get; set; }

    
    }

    [Table("pbookers")]
    public class User : Entity
    {
        [Column("name")]
        public String Name { get; set; }

        [Column("email")]
        public String Email { get; set; }

        [Column("user_id")]
        public String UserIdentifier { get; set; } /* Facebook identifier */

        [Column("Birthdate")]
        public DateTime BirthDate { get; set; }

        [InverseProperty("user")]
        public ICollection<Sharerelation> SharedWith { get; set; }



    }
}