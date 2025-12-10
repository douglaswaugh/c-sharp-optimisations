# C# Optimization Interview Preparation Plan

## Overview
A comprehensive list of 100 C# optimization problems and concepts, organized by topic area. Each item represents a specific optimization pattern, anti-pattern, or concept to understand.

---

## 1. Async/Await (15 items)

1. **Async void pitfall** - Why `async void` should only be used for event handlers (exceptions, fire-and-forget issues)
2. **ConfigureAwait(false)** - When and why to use it in library code to avoid deadlocks
3. **Task.Run vs async/await** - Understanding when Task.Run is appropriate vs unnecessary thread pool usage
4. **Async over sync anti-pattern** - Why wrapping sync code in Task.Run is usually wrong
5. **Sync over async anti-pattern** - Why calling .Result or .Wait() causes deadlocks and thread pool starvation
6. **ValueTask vs Task** - When to use ValueTask for hot paths that often complete synchronously
7. **Eliding async/await** - When it's safe to return Task directly vs awaiting
8. **IAsyncEnumerable** - Streaming async data instead of buffering entire collections
9. **CancellationToken propagation** - Properly threading cancellation through async chains
10. **Task.WhenAll vs sequential awaits** - Parallel async operations for independent work
11. **Async lazy initialization** - Using `Lazy<Task<T>>` or `AsyncLazy<T>` patterns
12. **SemaphoreSlim for async throttling** - Rate limiting concurrent async operations
13. **Async disposal (IAsyncDisposable)** - Properly cleaning up async resources
14. **Task.FromResult for cached results** - Avoiding allocations for known values
15. **Async state machine allocations** - Understanding what the compiler generates

---

## 2. HttpClient (10 items)

16. **HttpClient lifetime management** - Why `new HttpClient()` per request causes socket exhaustion
17. **IHttpClientFactory** - Proper HttpClient management with DNS rotation
18. **HttpClient socket exhaustion** - Understanding TIME_WAIT and port exhaustion
19. **Connection pooling** - How HttpClient reuses connections
20. **HttpCompletionOption.ResponseHeadersRead** - Streaming large responses instead of buffering
21. **Timeout configuration** - HttpClient.Timeout vs CancellationToken vs per-request timeouts
22. **HttpRequestMessage reuse** - Why you can't reuse request messages
23. **Compression handling** - Automatic decompression configuration
24. **DNS caching issues** - Why static HttpClient can have stale DNS
25. **Polly integration** - Retry policies and circuit breakers for resilience

---

## 3. Collections (15 items)

26. **List<T> capacity pre-allocation** - Avoiding repeated array resizing
27. **Dictionary<K,V> capacity** - Preventing rehashing with initial capacity
28. **HashSet<T> for lookups** - O(1) vs O(n) for Contains operations
29. **Array vs List<T>** - When arrays are more appropriate
30. **Span<T> for slicing** - Zero-allocation array/string slicing
31. **ArrayPool<T>** - Renting arrays to reduce GC pressure
32. **ImmutableArray vs ImmutableList** - Performance characteristics of immutable collections
33. **ConcurrentDictionary vs Dictionary + lock** - When each is appropriate
34. **Stack<T> and Queue<T>** - Using the right data structure
35. **LinkedList<T> misconceptions** - Why it's rarely the right choice in practice
36. **SortedDictionary vs SortedList** - Different performance profiles
37. **Collection expression allocations** - Understanding what `[]` and `[..items]` allocate
38. **ReadOnlySpan<T>** - For read-only slice operations
39. **Memory<T> vs Span<T>** - Heap vs stack constraints
40. **FrozenDictionary/FrozenSet (.NET 8+)** - Optimized immutable lookups

---

## 4. LINQ (15 items)

41. **LINQ allocation overhead** - Iterator allocations and closure captures
42. **Deferred execution pitfalls** - Multiple enumeration issues
43. **ToList() vs ToArray()** - When materialization is needed and which to choose
44. **Any() vs Count() > 0** - Short-circuit evaluation
45. **FirstOrDefault() vs SingleOrDefault()** - Performance and semantic differences
46. **Where().First() vs First(predicate)** - Avoiding intermediate iterators
47. **Select allocation** - Understanding what each LINQ operator allocates
48. **OrderBy stability and performance** - Sorting complexity
49. **Contains() on HashSet vs List** - Collection type matters for LINQ
50. **Avoiding LINQ in hot paths** - When to use loops instead
51. **GroupBy memory usage** - Buffering behavior
52. **AsEnumerable() vs AsQueryable()** - Preventing query translation issues
53. **LINQ to Objects vs LINQ to Entities** - Understanding evaluation location
54. **Aggregate() performance** - Custom aggregation without allocation
55. **DistinctBy, UnionBy (.NET 6+)** - Newer efficient LINQ methods

---

## 5. Memory Management (15 items)

56. **Value types vs reference types** - Stack vs heap allocation
57. **struct guidelines** - When to use structs (size, mutability, boxing)
58. **Large Object Heap (LOH)** - Objects >85KB, fragmentation issues
59. **GC generations** - Gen0, Gen1, Gen2 collection costs
60. **Object pooling** - ObjectPool<T> for expensive objects
61. **String interning** - String.Intern() for repeated strings
62. **StringBuilder vs string concatenation** - When StringBuilder matters
63. **stackalloc** - Stack allocation for small arrays
64. **ref struct limitations** - Stack-only types (Span<T>)
65. **Finalizers cost** - Why finalizers delay GC and should be avoided
66. **IDisposable pattern** - Deterministic cleanup of unmanaged resources
67. **Weak references** - Caching without preventing GC
68. **Memory<T> and IMemoryOwner<T>** - Memory lifetime management
69. **ArraySegment<T>** - Referencing array portions without copying
70. **GC.Collect() anti-pattern** - Why manual GC calls are usually wrong

---

## 6. Boxing/Unboxing (10 items)

71. **What is boxing** - Value type to object heap allocation
72. **Common boxing scenarios** - Non-generic collections, interfaces, string formatting
73. **Generic constraints to avoid boxing** - Using `where T : struct, IEquatable<T>`
74. **Dictionary key boxing** - Using proper IEqualityComparer<T>
75. **Enum boxing traps** - HasFlag, ToString(), dictionary keys
76. **Interface dispatch on structs** - Constrained calls vs boxing
77. **Nullable<T> boxing behavior** - How nullable value types box
78. **params object[] boxing** - Boxing in variadic methods
79. **string.Format and interpolation boxing** - Avoiding value type boxing in strings
80. **LINQ and boxing** - How LINQ can cause boxing with value types

---

## 7. Exception Handling (10 items)

81. **Exception cost** - Why exceptions are expensive (stack capture)
82. **Try-parse pattern** - Avoiding exceptions for expected failures
83. **Exception filtering** - `when` clause performance
84. **Rethrowing correctly** - `throw;` vs `throw ex;` (stack trace preservation)
85. **Exception allocation** - Creating exceptions allocates
86. **Expected vs exceptional** - Designing APIs to not throw for normal flow
87. **Result<T> pattern** - Alternatives to exceptions for expected failures
88. **ExceptionDispatchInfo** - Preserving stack traces across async boundaries
89. **First-chance exceptions** - Debugging performance with many caught exceptions
90. **AggregateException handling** - Proper handling of parallel/async exceptions

---

## 8. Concurrency (10 items)

91. **lock contention** - Impact of thread waiting on locks
92. **ReaderWriterLockSlim** - Multiple readers, single writer pattern
93. **Interlocked operations** - Lock-free atomic operations
94. **volatile keyword** - Memory barriers and visibility (and why it's rarely enough)
95. **ConcurrentQueue<T> vs Queue<T> + lock** - Lock-free collection performance
96. **SpinLock and SpinWait** - When spinning is better than blocking
97. **Thread pool starvation** - Symptoms and causes
98. **Parallel.ForEach partitioning** - Efficient parallel iteration
99. **Channel<T>** - Producer-consumer patterns
100. **Task.Yield()** - Preventing thread starvation in loops

---

## Suggested Learning Approach

1. **Review each item** - Read the concept and ensure you understand it
2. **See bad code** - Look at an anti-pattern example
3. **See good code** - Look at the optimized version
4. **Understand why** - Know the underlying reason for the optimization
5. **Know how to measure** - BenchmarkDotNet, profilers, memory analyzers

---

## Priority Areas for Interview

Based on common interview focus:
- **High priority**: Items 1-5, 16-18, 26-29, 41-45, 56-59, 71-74, 81-83, 91-93
- **Medium priority**: Items 6-10, 19-22, 30-35, 46-50, 60-65, 75-78, 84-87, 94-97
- **Good to know**: Remaining items

---

## Next Steps

Once approved, we'll work through these items with:
1. **Code examples** showing the anti-pattern
2. **Optimized versions** showing the fix
3. **Explanations** of why the optimization matters
4. **Benchmark comparisons** where relevant
