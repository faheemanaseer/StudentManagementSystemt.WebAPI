CREATE PROCEDURE GetAllStudents
AS
BEGIN
    SELECT 
        UId,
        Name,
        Email,
        Phone,
        Age,
        UserId
    FROM Students
END
