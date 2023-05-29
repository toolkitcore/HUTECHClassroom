﻿using EntityFrameworkCore.Repository.Extensions;
using HUTECHClassroom.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HUTECHClassroom.Application.Exercises.Commands.RemoveExerciseUser;

public record RemoveExerciseUserCommand(Guid Id, Guid UserId) : IRequest<Unit>;
public class RemoveExerciseUserCommandHandler : IRequestHandler<RemoveExerciseUserCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Exercise> _repository;

    public RemoveExerciseUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.Repository<Exercise>();
    }
    public async Task<Unit> Handle(RemoveExerciseUserCommand request, CancellationToken cancellationToken)
    {
        var query = _repository
            .SingleResultQuery()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
            .Include(i => i.Include(x => x.ExerciseUsers).ThenInclude(x => x.User))
            .AndFilter(x => x.Id == request.Id);

        var exercise = await _repository
            .SingleOrDefaultAsync(query, cancellationToken);

        if (exercise == null) throw new NotFoundException(nameof(Exercise), request.Id);

        var user = exercise.ExerciseUsers.SingleOrDefault(x => x.UserId == request.UserId);

        if (user == null) throw new NotFoundException(nameof(ApplicationUser), request.UserId);

        exercise.ExerciseUsers.Remove(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        _repository.RemoveTracking(exercise);

        return Unit.Value;
    }
}
