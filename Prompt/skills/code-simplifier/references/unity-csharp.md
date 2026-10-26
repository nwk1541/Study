# Unity and C# safeguards

Read this reference only when relevant to a proposed simplification. The points below are reasons to examine actual code paths, not instructions to add more defensive code.

## Unity object semantics

- `UnityEngine.Object` overloads `==` and `!=` to account for a destroyed native object whose managed wrapper remains. `is null`, `ReferenceEquals`, `?.`, and `??` do not use the same overloaded comparison. Do not substitute these expressions mechanically; consider static type and object lifetime.
- Preserve Unity lifecycle behavior (`Awake`, `OnEnable`, `OnDisable`, `OnDestroy`), component and pooled-object ownership, and main-thread requirements where relevant.
- Consider serialized field/type identity, `[SerializeField]`, `[SerializeReference]`, scene and prefab references, and assembly definition (`asmdef`) boundaries before structural changes.

## Async operations and Addressables

- Distinguish cooperative `CancellationToken` cancellation, invalidation of an earlier request (for example, via a generation counter), and cancellation of the underlying asset operation. A token check alone does not cancel an Addressables load. A version guard and token check can protect different observable behavior; inspect actual ordering and result-application paths before removing either.
- Determine handle, asset, and instance ownership and the correct release responsibility from the **specific API and its options**. `autoReleaseHandle` and `trackHandle` have different purposes; do not assume all operations require manual release, or that tracked instances are automatically released.
- Respect the resource's required lifetime, including success, failure, cancellation, and handoff to callers. Do not introduce premature or duplicate release, leaks, or use-after-release. Confirm whether an operation or its handle remains active after a caller stops awaiting it.

## Performance-sensitive code

- Avoid stylistic rewrites that introduce allocations, LINQ, closures, dispatch overhead, or repeated work in hot paths without demonstrated benefit. Preserve performance-related contracts; benchmark if the change could materially alter performance.
