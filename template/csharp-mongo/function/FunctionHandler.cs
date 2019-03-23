using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Function
{
    public class FunctionHandler
    {
        public Task<string> Handle(object input)
        {
            var response = new ResponseModel()
            {
                response = input,
                status = 201
            };
            
            return Task.FromResult(JsonConvert.SerializeObject(response));
        }
    }
}