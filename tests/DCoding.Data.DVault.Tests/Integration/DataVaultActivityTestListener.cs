using System.Diagnostics;

namespace DCoding.Data.DVault.Tests.Integration;

internal sealed class DataVaultActivityTestListener : IDisposable {
  private readonly ActivityListener _listener;
  private readonly List<Activity> _stoppedActivities = [];

  public DataVaultActivityTestListener(bool allDataRequested = true) {
    _listener = new ActivityListener {
      ShouldListenTo = source => string.Equals(source.Name, "DCoding.Data.DVault", StringComparison.Ordinal),
      Sample = (ref ActivityCreationOptions<ActivityContext> _) => allDataRequested
          ? ActivitySamplingResult.AllDataAndRecorded
          : ActivitySamplingResult.PropagationData,
      ActivityStopped = activity => _stoppedActivities.Add(activity),
    };

    ActivitySource.AddActivityListener(_listener);
  }

  public IReadOnlyList<Activity> StoppedActivities => _stoppedActivities;

  public void Dispose() {
    _listener.Dispose();
  }
}
