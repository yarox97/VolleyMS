using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VolleyMS.Core.Common;
using VolleyMS.Core.Shared;

namespace VolleyMS.Core.Models
{
    public class MediaFile : BaseEntity
    {
        private MediaFile()
            : base(Guid.Empty)
        {
        }

        private MediaFile(Guid id, string url, string storageId, string fileName, string contentType, long size)
            : base(id)
        {
            Url = url;
            StorageId = storageId;
            FileName = fileName;
            ContentType = contentType;
            Size = size;
        }


        public Guid Id { get; init; }
        public string Url { get; private set; }
        public string StorageId { get; private set; } // Cloudinary public id at this point
        public string FileName { get; private set; }
        public string ContentType { get; private set; }
        public long Size { get; private set; }
        public static Result<MediaFile> Create(string url, string storageId, string fileName, string contentType, long size)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return Result.Failure<MediaFile>(new Error(
                    "MediaFile.EmptyUrl",
                    "Media URL cannot be empty"));
            }

            if (string.IsNullOrWhiteSpace(storageId))
            {
                return Result.Failure<MediaFile>(new Error(
                    "MediaFile.EmptyStorageId",
                    "Storage ID cannot be empty"));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return Result.Failure<MediaFile>(new Error(
                    "MediaFile.EmptyFileName",
                    "File name cannot be empty"));
            }

            if (size <= 0)
            {
                return Result.Failure<MediaFile>(new Error(
                    "MediaFile.InvalidSize",
                    "File size must be greater than 0 bytes"));
            }

            var mediaFile = new MediaFile(
                Guid.NewGuid(),
                url,
                storageId,
                fileName,
                contentType,
                size
            );

            return Result.Success(mediaFile);
        }
    }
}
