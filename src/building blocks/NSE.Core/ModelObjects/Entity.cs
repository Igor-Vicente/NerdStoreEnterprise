using NSE.Core.Messages;

namespace NSE.Core.ModelObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        private List<Event> _notificacoes;
        public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();

        public void AdicionarEvento(Event evento)
        {
            _notificacoes = _notificacoes ?? new List<Event>();
            _notificacoes.Add(evento);
        }
        public void RemoverEvento(Event evento)
        {
            _notificacoes?.Remove(evento);
        }
        public void LimparEventos()
        {
            _notificacoes?.Clear();
        }

        #region Comporações
        public static bool operator ==(Entity left, Entity right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;

            return left.Equals(right);
        }
        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false; //compara referencia na memoria
            if (ReferenceEquals(this, obj)) return true; //compara referencia na memoria
            return Id.Equals(((Entity)obj).Id); //compara o valor
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }
        #endregion
    }
}
