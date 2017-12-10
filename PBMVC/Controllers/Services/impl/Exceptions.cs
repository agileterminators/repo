using System;

namespace PBMVC.Controllers.Services{

    public class OperationNotPermittedException : Exception {
        public OperationNotPermittedException() : base("Operation not Permitted!") {
            
        }
    }
}