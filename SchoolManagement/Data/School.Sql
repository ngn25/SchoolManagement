
INSERT INTO Teachers ( Name, Email, PhoneNumber)
VALUES 
    ( 'DrAlexander Harris', 'alexander.harris@school.edu', '09121112323'),
    ( 'ProfIsabellaMartinez', 'isabella.martinez@school.edu', '09112355566'),
    ( 'MrJamesWilson', 'james.wilson@school.edu', '555-1103'),
    ( 'DrSophiaAnderson', 'sophia.anderson@school.edu', '09125689741'),
    ( 'MsBenjaminThomas', 'benjamin.thomas@school.edu', NULL),
    ( 'ProfOliviaJackson', 'olivia.jackson@school.edu', '09336547410'),
    ( 'DrWilliamWhite', 'william.white@school.edu', '09145588997'),
    ( 'MrsCharlotteLee', 'charlotte.lee@school.edu', '09158741236'),
    ( 'MrDanielClark', 'daniel.clark@school.edu', NULL),
    ( 'ProfAmeliaLewis', 'amelia.lewis@school.edu', '09124578963');


INSERT INTO Students ( Name, DateOfBirth, Email, PhoneNumber)
VALUES 
    ( 'EmmaJohnson', '2006-03-15', 'emma.j@student.com', '09135625874'),
    ( 'LiamSmith', '2005-07-22', 'liam.s@student.com', '09125869354'),
    ( 'NoahWilliams', '2007-01-10', 'noah.w@student.com', NULL),
    ('OliviaBrown', '2006-09-05', 'olivia.b@student.com', '09137481520'),
    ( 'EthanJones', '2005-11-30', 'ethan.j@student.com', '09124488996'),
    ( 'AvaGarcia', '2007-04-18', 'ava.g@student.com', '09121212142'),
    ( 'SophiaMiller', '2006-06-25', 'sophia.m@student.com', NULL),
    ( 'MasonDavis', '2005-12-12', 'mason.d@student.com', '09125896354'),
    ( 'IsabellaRodriguez', '2007-08-08', 'isabella.r@student.com', '09174523698'),
    ('LucasMartinez', '2006-10-20', 'lucas.m@student.com', '09124548885');


INSERT INTO Courses (Name, TeacherId)
VALUES 
('CalculusIII', 1),
('PhysicsII', 3),
('IntroductiontoProgramming', 4),
('OrganicChemistry', 6),
('WorldHistory', 8);


