/********************************************************************++
Copyright (c) Microsoft Corporation.  All rights reserved.
--********************************************************************/

using System;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Runtime.Serialization;
using System.Diagnostics.CodeAnalysis;

#if CORECLR
// Use stub for SystemException, SerializationInfo, SerializableAttribute and Serializable
// It is needed for WriteErrorException which ONLY used in Write-Error cmdlet.
using Microsoft.PowerShell.CoreClr.Stubs;
#endif

namespace Microsoft.PowerShell.Commands
{
    #region WriteDebugCommand
    /// <summary>
    /// This class implements Write-Debug command
    /// 
    /// </summary>
    [Cmdlet("Write", "Debug", HelpUri = "https://go.microsoft.com/fwlink/?LinkID=113424", RemotingCapability = RemotingCapability.None)]
    public sealed class WriteDebugCommand : PSCmdlet
    {
        /// <summary>
        /// Message to be sent and processed if debug mode is on.
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [AllowEmptyString]
        [Alias("Msg")]
        public string Message { get; set; } = null;


        /// <summary>
        /// This method implements the ProcessRecord method for Write-Debug command
        /// </summary>
        protected override void ProcessRecord()
        {
            //
            // The write-debug command must use the script's InvocationInfo rather than its own,
            // so we create the DebugRecord here and fill it up with the appropriate InvocationInfo;
            // then, we call the command runtime directly and pass this record to WriteDebug().
            //
            MshCommandRuntime mshCommandRuntime = this.CommandRuntime as MshCommandRuntime;

            if (mshCommandRuntime != null)
            {
                DebugRecord record = new DebugRecord(Message);

                InvocationInfo invocationInfo = GetVariableValue(SpecialVariables.MyInvocation) as InvocationInfo;

                if (invocationInfo != null)
                {
                    record.SetInvocationInfo(invocationInfo);
                }

                mshCommandRuntime.WriteDebug(record);
            }
            else
            {
                WriteDebug(Message);
            }
        }//processrecord
    }//WriteDebugCommand
    #endregion WriteDebugCommand

    #region WriteVerboseCommand
    /// <summary>
    /// This class implements Write-Verbose command
    /// 
    /// </summary>
    [Cmdlet("Write", "Verbose", HelpUri = "https://go.microsoft.com/fwlink/?LinkID=113429", RemotingCapability = RemotingCapability.None)]
    public sealed class WriteVerboseCommand : PSCmdlet
    {
        /// <summary>
        /// Message to be sent if verbose messages are being shown.
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [AllowEmptyString]
        [Alias("Msg")]
        public string Message { get; set; } = null;


        /// <summary>
        /// This method implements the ProcessRecord method for Write-verbose command
        /// </summary>
        protected override void ProcessRecord()
        {
            //
            // The write-verbose command must use the script's InvocationInfo rather than its own,
            // so we create the VerboseRecord here and fill it up with the appropriate InvocationInfo;
            // then, we call the command runtime directly and pass this record to WriteVerbose().
            //
            MshCommandRuntime mshCommandRuntime = this.CommandRuntime as MshCommandRuntime;

            if (mshCommandRuntime != null)
            {
                VerboseRecord record = new VerboseRecord(Message);

                InvocationInfo invocationInfo = GetVariableValue(SpecialVariables.MyInvocation) as InvocationInfo;

                if (invocationInfo != null)
                {
                    record.SetInvocationInfo(invocationInfo);
                }

                mshCommandRuntime.WriteVerbose(record);
            }
            else
            {
                WriteVerbose(Message);
            }
        }//processrecord
    }//WriteVerboseCommand
    #endregion WriteVerboseCommand

    #region WriteWarningCommand
    /// <summary>
    /// This class implements Write-Warning command
    /// 
    /// </summary>
    [Cmdlet("Write", "Warning", HelpUri = "https://go.microsoft.com/fwlink/?LinkID=113430", RemotingCapability = RemotingCapability.None)]
    public sealed class WriteWarningCommand : PSCmdlet
    {
        /// <summary>
        /// Message to be sent if warning messages are being shown.
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [AllowEmptyString]
        [Alias("Msg")]
        public string Message { get; set; } = null;


        /// <summary>
        /// This method implements the ProcessRecord method for Write-Warning command
        /// </summary>
        protected override void ProcessRecord()
        {
            //
            // The write-warning command must use the script's InvocationInfo rather than its own,
            // so we create the WarningRecord here and fill it up with the appropriate InvocationInfo;
            // then, we call the command runtime directly and pass this record to WriteWarning().
            //
            MshCommandRuntime mshCommandRuntime = this.CommandRuntime as MshCommandRuntime;

            if (mshCommandRuntime != null)
            {
                WarningRecord record = new WarningRecord(Message);

                InvocationInfo invocationInfo = GetVariableValue(SpecialVariables.MyInvocation) as InvocationInfo;

                if (invocationInfo != null)
                {
                    record.SetInvocationInfo(invocationInfo);
                }

                mshCommandRuntime.WriteWarning(record);
            }
            else
            {
                WriteWarning(Message);
            }
        }//processrecord
    }//WriteWarningCommand
    #endregion WriteWarningCommand

    #region WriteInformationCommand
    /// <summary>
    /// This class implements Write-Information command
    /// 
    /// </summary>
    [Cmdlet(VerbsCommunications.Write, "Information", HelpUri = "https://go.microsoft.com/fwlink/?LinkId=525909", RemotingCapability = RemotingCapability.None)]
    public sealed class WriteInformationCommand : PSCmdlet
    {
        /// <summary>
        /// Object to be sent to the Information stream.
        /// </summary>
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
        [Alias("Msg")]
        public Object MessageData { get; set; }

        /// <summary>
        /// Any tags to be associated with this information
        /// </summary>
        [Parameter(Position = 1)]
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] Tags { get; set; }

        /// <summary>
        /// This method implements the processing of the Write-Information command
        /// </summary>
        protected override void BeginProcessing()
        {
            if (Tags != null)
            {
                foreach (string tag in Tags)
                {
                    if (tag.StartsWith("PS", StringComparison.OrdinalIgnoreCase))
                    {
                        ErrorRecord er = new ErrorRecord(
                            new InvalidOperationException(StringUtil.Format(UtilityCommonStrings.PSPrefixReservedInInformationTag, tag)),
                            "PSPrefixReservedInInformationTag", ErrorCategory.InvalidArgument, tag);
                        ThrowTerminatingError(er);
                    }
                }
            }
        }

        /// <summary>
        /// This method implements the ProcessRecord method for Write-Information command
        /// </summary>
        protected override void ProcessRecord()
        {
            WriteInformation(MessageData, Tags);
        }

    }//WriteInformationCommand

    #endregion WriteInformationCommand


    #region WriteOrThrowErrorCommand

    /// <summary>
    /// This class implements the Write-Error command
    /// </summary>
    public class WriteOrThrowErrorCommand : PSCmdlet
    {
        /// <summary>
        /// ErrorRecord.Exception -- if not specified, ErrorRecord.Exception is
        /// System.Exception.
        /// </summary>
        [Parameter(ParameterSetName = "WithException", Mandatory = true)]
        public Exception Exception { get; set; } = null;

        /// <summary>
        /// If Exception is specified, this is ErrorRecord.ErrorDetails.Message.
        /// Otherwise, the Exception is System.Exception, and this is
        /// Exception.Message.
        /// </summary>
        [Parameter(Position = 0, ParameterSetName = "NoException", Mandatory = true, ValueFromPipeline = true)]
        [Parameter(ParameterSetName = "WithException")]
        [AllowNull]
        [AllowEmptyString]
        [Alias("Msg")]
        public string Message { get; set; } = null;

        /// <summary>
        /// If Exception is specified, this is ErrorRecord.ErrorDetails.Message.
        /// Otherwise, the Exception is System.Exception, and this is
        /// Exception.Message.
        /// </summary>
        [Parameter(ParameterSetName = "ErrorRecord", Mandatory = true)]
        public ErrorRecord ErrorRecord { get; set; } = null;

        /// <summary>
        /// ErrorRecord.CategoryInfo.Category
        /// </summary>
        [Parameter(ParameterSetName = "NoException")]
        [Parameter(ParameterSetName = "WithException")]
        public ErrorCategory Category { get; set; } = ErrorCategory.NotSpecified;

        /// <summary>
        /// ErrorRecord.ErrorId
        /// </summary>
        [Parameter(ParameterSetName = "NoException")]
        [Parameter(ParameterSetName = "WithException")]
        public string ErrorId { get; set; } = "";

        /// <summary>
        /// ErrorRecord.TargetObject
        /// </summary>
        [Parameter(ParameterSetName = "NoException")]
        [Parameter(ParameterSetName = "WithException")]
        public object TargetObject { get; set; } = null;

        /// <summary>
        /// ErrorRecord.ErrorDetails.RecommendedAction
        /// </summary>
        [Parameter]
        public string RecommendedAction { get; set; } = "";

        /* 2005/01/25 removing throw-error
        /// <summary>
        /// If true, this is throw-error.  Otherwise, this is write-error.
        /// </summary>
        internal bool _terminating = false;
        */

        /// <summary>
        /// ErrorRecord.CategoryInfo.Activity
        /// </summary>
        [Parameter]
        [Alias("Activity")]
        public string CategoryActivity { get; set; } = "";

        /// <summary>
        /// ErrorRecord.CategoryInfo.Reason
        /// </summary>
        [Parameter]
        [Alias("Reason")]
        public string CategoryReason { get; set; } = "";

        /// <summary>
        /// ErrorRecord.CategoryInfo.TargetName
        /// </summary>
        [Parameter]
        [Alias("TargetName")]
        public string CategoryTargetName { get; set; } = "";

        /// <summary>
        /// ErrorRecord.CategoryInfo.TargetType
        /// </summary>
        [Parameter]
        [Alias("TargetType")]
        public string CategoryTargetType { get; set; } = "";


        /// <summary>
        /// Write an error to the output pipe, or throw a terminating error.
        /// </summary>
        protected override void ProcessRecord()
        {
            ErrorRecord errorRecord = this.ErrorRecord;
            if (null != errorRecord)
            {
                // copy constructor
                errorRecord = new ErrorRecord(errorRecord, null);
            }
            else
            {
                Exception e = this.Exception;
                string msg = Message;
                if (null == e)
                {
                    e = new WriteErrorException(msg);
                }
                string errid = ErrorId;
                if (String.IsNullOrEmpty(errid))
                {
                    errid = e.GetType().FullName;
                }
                errorRecord = new ErrorRecord(
                    e,
                    errid,
                    Category,
                    TargetObject
                    );

                if ((null != this.Exception && !String.IsNullOrEmpty(msg)))
                {
                    errorRecord.ErrorDetails = new ErrorDetails(msg);
                }
            }

            string recact = RecommendedAction;
            if (!String.IsNullOrEmpty(recact))
            {
                if (null == errorRecord.ErrorDetails)
                {
                    errorRecord.ErrorDetails = new ErrorDetails(errorRecord.ToString());
                }
                errorRecord.ErrorDetails.RecommendedAction = recact;
            }

            if (!String.IsNullOrEmpty(CategoryActivity))
                errorRecord.CategoryInfo.Activity = CategoryActivity;
            if (!String.IsNullOrEmpty(CategoryReason))
                errorRecord.CategoryInfo.Reason = CategoryReason;
            if (!String.IsNullOrEmpty(CategoryTargetName))
                errorRecord.CategoryInfo.TargetName = CategoryTargetName;
            if (!String.IsNullOrEmpty(CategoryTargetType))
                errorRecord.CategoryInfo.TargetType = CategoryTargetType;

            /* 2005/01/25 removing throw-error
            if (_terminating)
            {
                ThrowTerminatingError(errorRecord);
            }
            else
            {
            */

            // 2005/07/14-913791 "write-error output is confusing and misleading"
            // set InvocationInfo to the script not the command
            InvocationInfo myInvocation = GetVariableValue(SpecialVariables.MyInvocation) as InvocationInfo;
            if (null != myInvocation)
            {
                errorRecord.SetInvocationInfo(myInvocation);
                errorRecord.PreserveInvocationInfoOnce = true;
                if (!String.IsNullOrEmpty(CategoryActivity))
                    errorRecord.CategoryInfo.Activity = CategoryActivity;
                else
                    errorRecord.CategoryInfo.Activity = "Write-Error";
            }

            WriteError(errorRecord);
            /*
            }
            */
        }//processrecord
    }//WriteOrThrowErrorCommand

    /// <summary>
    /// This class implements Write-Error command
    /// </summary>
    [Cmdlet("Write", "Error", DefaultParameterSetName = "NoException",
        HelpUri = "https://go.microsoft.com/fwlink/?LinkID=113425", RemotingCapability = RemotingCapability.None)]
    public sealed class WriteErrorCommand : WriteOrThrowErrorCommand
    {
        /// <summary>
        /// constructor
        /// </summary>
        public WriteErrorCommand()
        {
        }
    }

    /* 2005/01/25 removing throw-error
        /// <summary>
        /// This class implements Write-Error command
        /// </summary>
        [Cmdlet("Throw", "Error", DefaultParameterSetName = "NoException")]
        public sealed class ThrowErrorCommand : WriteOrThrowErrorCommand
        {
            /// <summary>
            /// constructor
            /// </summary>
            public ThrowErrorCommand()
            {
                using (tracer.TraceConstructor(this))
                {
                    _terminating = true;
                }
            }
        }
    */

    #endregion WriteOrThrowErrorCommand

    #region WriteErrorException
    /// <summary>
    /// The write-error cmdlet uses WriteErrorException
    /// when the user only specifies a string and not
    /// an Exception or ErrorRecord.
    /// </summary>
    [Serializable]
    public class WriteErrorException : SystemException
    {
        #region ctor
        /// <summary>
        /// Constructor for class WriteErrorException
        /// </summary>
        /// <returns> constructed object </returns>
        public WriteErrorException()
            : base(StringUtil.Format(WriteErrorStrings.WriteErrorException))
        {
        }

        /// <summary>
        /// Constructor for class WriteErrorException
        /// </summary>
        /// <param name="message">  </param>
        /// <returns> constructed object </returns>
        public WriteErrorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor for class WriteErrorException
        /// </summary>
        /// <param name="message">  </param>
        /// <param name="innerException">  </param>
        /// <returns> constructed object </returns>
        public WriteErrorException(string message,
                                          Exception innerException)
            : base(message, innerException)
        {
        }
        #endregion ctor

        #region Serialization
        /// <summary>
        /// Serialization constructor for class WriteErrorException
        /// </summary>
        /// <param name="info"> serialization information </param>
        /// <param name="context"> streaming context </param>
        /// <returns> constructed object </returns>
        protected WriteErrorException(SerializationInfo info,
                                      StreamingContext context)
            : base(info, context)
        {
        }
        #endregion Serialization
    } // WriteErrorException
    #endregion WriteErrorException
} //namespace



