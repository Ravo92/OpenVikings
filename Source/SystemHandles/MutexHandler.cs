namespace OpenVikings.SystemHandles
{
    internal class MutexHandler
    {
        public enum MutexCreationResult
        {
            Success,
            AlreadyExists,
            CreationFailed
        }

        private static Mutex? mutex = null;

        /// <summary>
        /// Attempts to create a named mutex and returns the result as an enum.
        /// </summary>
        /// <param name="name">The name of the mutex.</param>
        /// <returns>MutexCreationResult indicating the outcome of the operation.</returns>
        public static MutexCreationResult TryCreateMutex(string name)
        {
            if (mutex != null)
            {
                return MutexCreationResult.Success;
            }

            try
            {
                mutex = new Mutex(false, name, out bool createdNew);

                if (createdNew)
                {
                    return MutexCreationResult.Success;
                }
                else
                {
                    return MutexCreationResult.AlreadyExists;
                }
            }
            catch (Exception)
            {
                return MutexCreationResult.CreationFailed;
            }
        }
    }
}