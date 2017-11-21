namespace WebApi.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class TodoItem: BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsComplete { get; set; }
    }
}
