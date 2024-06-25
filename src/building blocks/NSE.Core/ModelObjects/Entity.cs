namespace NSE.Core.ModelObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

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
    }
}
