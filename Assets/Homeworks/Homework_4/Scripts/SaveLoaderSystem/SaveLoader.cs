using Zenject;

namespace SaveLoad
{
    public abstract class SaveLoader<TService, TData> : ISaveLoader
    {
        private TService _service;


        [Inject]
        public void Construct(TService service)
        {
            _service = service;
        }

        public void LoadData(IGameRepository repository)
        {
            if (repository.TryGetData(out TData data))
            {
                SetupData(data, _service);
            }
            else
            {
                SetupByDefault(_service);
            }
        }

        public void SaveData(IGameRepository repository)
        {
            TData data = ConvertToData(_service);
            repository.SetData(data);
        }

        protected abstract void SetupByDefault(TService service);
        protected abstract void SetupData(TData data, TService service);
        protected abstract TData ConvertToData(TService service);
    }
}