CREATE PROCEDURE GetStudentByUserId
    @UserId INT
AS
BEGIN
    SELECT TOP 1 
        UId,
        Name,
        Email,
        Phone,
        Age,
        UserId
    FROM Students
    WHERE UserId = @UserId
END
