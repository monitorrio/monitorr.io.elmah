using System;
using System.Collections;
using Elmah;

namespace monitorr.io.elmah
{
    public class ErrorLog : Elmah.ErrorLog
    {
        private readonly string _logId;

        public ErrorLog(IDictionary config)
        {
            if (config == null)
            {
                throw new NullReferenceException("config");
            }

            _logId = (string)config["LogId"] ?? string.Empty;
        }

        public override string Log(Error error)
        {
            if (error == null)
            {
                throw new NullReferenceException("error");
            }

            var model = ErrorModelCreator.Create(error, _logId);

            var client = new MonitorrClient();

            client.Log(model);

            return model.Guid;
        }

        public override string Name
        {
            get { return "Monitorr Error Log"; }
        }

        public override ErrorLogEntry GetError(string id)
        {
            throw new NotImplementedException();
        }

        public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
        {
            throw new NotImplementedException();
        }


    }
}
