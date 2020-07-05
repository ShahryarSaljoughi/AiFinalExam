using System;
using System.Collections.Generic;
using System.Linq;
using CSP.Entities;
using CSP.Helper;
using Microsoft.VisualBasic;

namespace CSP
{
    public class CspSolver
    {
        public List<Constraint> Constraints { get; set; }
        public int NeededSensorsForEachTargetNo { get; set; }
        private List<Variable> Variables { get; set; }
        private List<DomainValue> DomainValues { get; set; }
        private int SensorsNo { get; set; }
        private int TargetNo { get; set; }

        public CspSolver(int neededSensorsForEachTargetNo, int[,] visibility, int[,] communications)
        {
            NeededSensorsForEachTargetNo = neededSensorsForEachTargetNo;
            SensorsNo = visibility.GetLength(0);
            TargetNo = visibility.GetLength(1);
            PopulateDomainValues(visibility);
            InitializeVariables(visibility);
            PopulateVariableCandidates(visibility);
            PopulateConstraints(visibility, communications);
        }


        public Result Solve()
        {
            var initialState = new NodeState() {Variables = this.Variables.ToList()};
            return SolveRecursively(initialState);
        }

        private Result SolveRecursively(NodeState currentState)
        {
            if (currentState.Variables.All(variable => variable.IsAssigned))
                return new Result() {AssignedVariables = currentState.Variables.ToDictionary(v => v.Name)};

            var variableToAssign = GetNextVariable(currentState);
            var stateBeforeGoingDeeper = currentState.Clone();
            foreach (var value in GetValueFor(variableToAssign))
            {
                if (!IsConsistent(currentState, variableToAssign, value)) continue;
                variableToAssign.AssignedValue = value.Value;
                try
                {
                    return SolveRecursively(currentState);
                }
                catch (CspPathFailure)
                {
                    currentState.GoBackToState(stateBeforeGoingDeeper);
                }
            }

            throw new CspPathFailure();
        }

        private bool IsConsistent(NodeState currentState, Variable variableToAssign, in DomainValue value)
        {
            var stateToCheck = currentState.Clone();
            var sensor = stateToCheck.Variables.GetByName(variableToAssign.Name);
            sensor.AssignedValue = value.Value;
            sensor.IsAssigned = true;
            //todo: do forward check
            return Constraints.All(constraint => constraint(stateToCheck.Variables));
        }

        private IEnumerable<DomainValue> GetValueFor(Variable variableToAssign)
        {
            // todo: apply least constraining value here
            foreach (var value in variableToAssign.Candidates)
            {
                yield return value;
            }
        }

        private Variable GetNextVariable(NodeState state)
        {
            return state.Variables
                .Where(v => v.IsAssigned == false)
                .OrderByDescending(v => v.Candidates.Count())
                // todo: Degree Heuristic
                .First();
        }

        private void PopulateDomainValues(int[,] visibility)
        {
            DomainValues = new List<DomainValue>();
            foreach (var i in Enumerable.Range(0, TargetNo))
            {
                DomainValues.Add(new DomainValue() {Id = i, Value = $"t{i}"});
            }
        }

        private void PopulateConstraints(int[,] visibility, int[,] communications)
        {
            // each sensor has at least one candidate value, or is already assigned.
            var rule1 = new Constraint((variables) =>
                variables.All(variable => variable.IsAssigned || variable.Candidates.Any()));

            // collaborating sensors are in each others'  access range
            var rule2 = new Constraint((variables) =>
            {
                var collaboratingSensorGroups =
                    variables.Where(sensor => sensor.IsAssigned).GroupBy(sensor => sensor.AssignedValue);

                return collaboratingSensorGroups.All(group =>
                {
                    bool sensorsAreConnected = true;
                    foreach (var sensor1 in group)
                    {
                        foreach (var sensor2 in group)
                        {
                            if (sensor1.Id == sensor2.Id) continue;
                            if (communications[sensor1.Id, sensor2.Id] != 1) sensorsAreConnected = false;
                        }
                    }

                    return sensorsAreConnected;
                });
            });

            // no target is being tracked by more than K sensors
            var rule3 = new Constraint((variables) =>
            {
                var collaboratingSensorGroups =
                    variables.Where(sensor => sensor.IsAssigned).GroupBy(sensor => sensor.AssignedValue);
                var result = collaboratingSensorGroups.All(group => group.Count() <= NeededSensorsForEachTargetNo);
                return result;
            });

            // there are enough sensors for each target
            var rule4 = new Constraint((variables) =>
            {
                return DomainValues.All(target =>
                {
                    var sensorsAlreadyTracking = variables.Where(sensor => sensor.AssignedValue == target.Value);
                    var sensorsAlreadyTrackingTargetNo = sensorsAlreadyTracking.Count();
                    var sensorsRemainedForTargetNo = variables.Count(sensor =>
                    {
                        var sensorIsConnectedToOthers = true;
                        foreach (var sensorItem in sensorsAlreadyTracking)
                        {
                            if (communications[sensorItem.Id, sensor.Id] == 0) sensorIsConnectedToOthers = false;
                        }

                        if (!sensorIsConnectedToOthers) return false;

                        return !sensor.IsAssigned && sensor.Candidates.Select(c => c.Value).Contains(target.Value);
                    });
                    return sensorsAlreadyTrackingTargetNo + sensorsRemainedForTargetNo >= NeededSensorsForEachTargetNo;
                });
            });

            this.Constraints = new List<Constraint>();
            this.Constraints.AddRange(new[] {rule1, rule2, rule3, rule4});
        }

        private void PopulateVariableCandidates(int[,] visibility)
        {
            foreach (var sensor in Variables)
            {
                sensor.Candidates = DomainValues.Where(domainValue => visibility[sensor.Id, domainValue.Id] == 1)
                    .ToList();
            }
        }

        private void InitializeVariables(int[,] visibility)
        {
            Variables = new List<Variable>();
            for (int i = 0; i < SensorsNo; i++)
            {
                var sensor = new Variable() {Name = $"s{i}", Id = i};
                this.Variables.Add(sensor);
            }
        }
    }
}