using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace ADE_ManagementSystem.Models.Global
{
    public enum FailureReasonCode
    {
        General,
        AccountLevel
    }

    public class ResponseResultViewModel
    {
        private readonly IList<string> _messages = new List<string>();

        /// <summary>
        /// Convenience wrapper if you only have a single message will modify the Messages collection
        /// </summary>
        public string Message
        {
            get
            {
                return _messages != null ? _messages.FirstOrDefault() : null;
            }
            set
            {
                Messages = new List<string> { value };
            }
        }

        public ResponseResultViewModel AddMessage(string format, params object[] values)
        {
            if (Messages == null)
            {
                Messages = new List<string>();
            }

            var message = string.Format(format ?? "", values);
            if (!string.IsNullOrEmpty(message))
            {
                Messages.Add(message);
            }
            return this;
        }

        public ICollection<string> Messages
        {
            get { return _messages; }
            set
            {
                value = value ?? new List<string>();
                _messages.Clear();
                foreach (var m in value)
                {
                    AddMessage(m);
                }
            }
        }


        public bool Success { get; set; }
        public FailureReasonCode Reason { get; set; }

        public ResponseResultViewModel() : this("")
        {
        }

        public ResponseResultViewModel(string message)
            : this(new List<string> { message })
        {
        }

        public ResponseResultViewModel(IEnumerable<string> messages)
        {
            messages = messages ?? new List<string>();
            Messages = messages.ToList();
            Reason = FailureReasonCode.General;
        }

        public ResponseResultViewModel(ModelStateDictionary modelState)
        {
            if (modelState != null && modelState.Any())
            {
                Messages = modelState.Values
                    .SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                    .ToList();
                Failure();
            }
        }

        //builder-style items to make creating these easier
        public ResponseResultViewModel Succeed()
        {
            Success = true;
            return this;
        }
        public ResponseResultViewModel Failure()
        {
            Success = false;
            return this;
        }

        public ResponseResultViewModel Failure(FailureReasonCode reason)
        {
            Reason = reason;
            return Failure();
        }
    }

    public class ResponseResultWithEntityViewModel<TViewModel> : ResponseResultViewModel
        where TViewModel : class
    {
        public TViewModel Entity { get; set; }

        public ResponseResultWithEntityViewModel() : base()
        {
        }

        public ResponseResultWithEntityViewModel(string message) : base(message)
        {
        }

        public ResponseResultWithEntityViewModel(IEnumerable<string> messages) : base(messages)
        {
        }
    }
}