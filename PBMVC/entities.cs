using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace PicBook
{

    public abstract class Entity: TableEntity
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
        [Column("image_id")]
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

        public Album() {
            Visibility = Visibility.Private;
            this.SharedWith = new List<Sharerelation>();
        }
        public Album(Guid id, string name) {
            this.ID = id;
            this.Title = name;
            this.SharedWith = new List<Sharerelation>();
            Visibility = Visibility.Private;
        }

        public void ShareWith(User user) {
            this.Visibility = Visibility.Shared;
            this.SharedWith.Add(new Sharerelation(user,this));
        }

    }

    [Table("Sharerelation")]
    public class Sharerelation : TableEntity
    {
        [Key]
        [Column("userid")]
        public User user { get; set; }

        [Key]
        [Column("albumid")]
        public Album album { get; set; }

        public Sharerelation() { }

        public Sharerelation(User user, Album album)
        {
            this.album = album;
            this.user = user;
        }

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

        [InverseProperty("Owner")]
        public ICollection<Album> Albums { get; set; }

        public User() {}

        public User(Guid id, string name, string email) {
            this.ID = id;
            this.Name = name;
            this.Email = email;
        }
    }
}