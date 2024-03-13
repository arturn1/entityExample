namespace EFCoreRelationshipsTutorial.Helpers
{
    public interface IMapper
    {
        void Map<Source, Destination>(Source source, Destination destination);
    }

    public class Mapper : IMapper
    {
        public void Map<Source, Destination>(Source source, Destination destination)
        {
            var sourceType = typeof(Source);
            var destinationType = typeof(Destination);

            var attr = sourceType.Attributes;
            var attr1 = sourceType.GetField("RpgClass");
            var attr2 = sourceType.GetType();

            var allowedProperties = GetAllowedProperties(source);

            foreach (var propertyName in allowedProperties)
            {
                var sourceProperty = sourceType.GetProperty(propertyName);
                var destinationProperty = destinationType.GetProperty(propertyName);

                if (sourceProperty != null && destinationProperty != null)
                {
                        var sourceValue = sourceProperty.GetValue(source);
                        if(!string.IsNullOrEmpty(sourceValue!.ToString()))
                            destinationProperty.SetValue(destination, sourceValue);
                }
            }
        }

        private string[] GetAllowedProperties<T>(T source)
        {
            var sourceType = typeof(T);
            var sourceProperties = sourceType.GetProperties();

            var allowedProperties = sourceProperties
                .Where(property =>
                {
                    var value = property.GetValue(source);
                    return value != null && property.Name != "Errors" && property.Name != "id" && property.Name != "isValid" && !string.IsNullOrEmpty(property.Name);
                })
                .Select(property => property.Name)
                .ToArray();

            return allowedProperties;
        }
    }
}