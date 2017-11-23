using System;

namespace PBMVC.Controllers.Services{

    class OperationNotPermittedException : Exception {
        public OperationNotPermittedException() : base("Operation not Permitted!") {
            
        }
    }
}