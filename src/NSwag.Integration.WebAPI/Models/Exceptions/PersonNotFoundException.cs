﻿using System;
using Newtonsoft.Json;

namespace NSwag.Integration.WebAPI.Models.Exceptions
{
    public class PersonNotFoundException : Exception
    {
        [JsonProperty("id")]
        public Guid Id { get; private set; }

        public PersonNotFoundException(Guid id)
        {
            Id = id;
        }
    }
}