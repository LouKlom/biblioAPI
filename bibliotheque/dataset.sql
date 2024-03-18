INSERT INTO `Auteurs` (`Id`, `FirstName`, `LastName`) VALUES
(1, 'string', 'string'),
(2, 'string', 'string');

INSERT INTO `Clients` (`Id`, `Name`, `Mail`, `Phone`) VALUES
(1, 'string', 'string', 'string'),
(2, 'string', 'string', 'string');

INSERT INTO `Medias` (`Id`, `Name`, `Description`, `Reserved`, `Edition`, `DateSortie`, `AuteurId`) VALUES
(1, 'string', 'string', 0, 'string', '2024-03-18 17:32:23.175000', 1),
(2, 'string', 'string', 0, 'string', '2024-03-18 17:32:23.175000', 1),
(3, 'string', 'string', 0, 'string', '2024-03-18 17:32:23.175000', 2),
(4, 'string', 'string', 0, 'string', '2024-03-18 17:32:23.175000', 2);

INSERT INTO `Reservations` (`Id`, `ClientId`, `MediaId`, `DateDebut`, `DateFin`) VALUES
(1, 1, 1, '2024-03-18 17:32:23.175000', NULL),
(2, 1, 2, '2024-03-18 17:32:23.175000', NULL),
(3, 2, 3, '2024-03-18 17:32:23.175000', NULL),
(4, 2, 4, '2024-03-18 17:32:23.175000', NULL);
