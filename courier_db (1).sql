-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 27, 2025 at 04:32 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `courier_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `customers`
--

CREATE TABLE `customers` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `phone` varchar(20) NOT NULL,
  `email` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `customers`
--

INSERT INTO `customers` (`id`, `name`, `phone`, `email`) VALUES
(1, 'ali', '0923456', 'ali@gmail.com');

-- --------------------------------------------------------

--
-- Table structure for table `packages`
--

CREATE TABLE `packages` (
  `tracking_number` varchar(50) NOT NULL,
  `sender` varchar(100) NOT NULL,
  `recipient` varchar(100) NOT NULL,
  `recipient_email` varchar(100) DEFAULT NULL,
  `destination` varchar(100) NOT NULL,
  `status` varchar(50) NOT NULL,
  `dispatch_date` datetime NOT NULL,
  `updated_at` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `packages`
--

INSERT INTO `packages` (`tracking_number`, `sender`, `recipient`, `recipient_email`, `destination`, `status`, `dispatch_date`, `updated_at`) VALUES
('01', 'as', 'fdsfigflg1323243434343', 'da', '---', 'Dispatched', '2025-03-27 22:11:06', NULL),
('1', '1', '', NULL, '', 'Out for Delivery', '2025-03-27 19:00:23', '2025-03-27 20:06:37'),
('12', 'ako', 'ikaw', NULL, 'mars', 'Out for Delivery', '2025-03-26 09:37:08', '2025-03-27 20:13:03'),
('2', 'saa', 'soo', NULL, 'samar', 'Dispatched', '2025-03-27 20:45:14', NULL),
('22', 'car', 'john', 'carloyacial@gmail.com', 'cad', 'Out for Delivery', '2025-03-27 22:29:18', '2025-03-27 22:29:28'),
('23', 'sa', 'carlo', 'carloyacial@gmail.com', 'samar', 'Delayed', '2025-03-27 22:11:54', '2025-03-27 23:16:01'),
('24', 'yu', 'carlo', 'carloyacial@gmail.com', 'etivac', 'Out for Delivery', '2025-03-27 22:23:41', '2025-03-27 22:23:54'),
('25', 'carlo', 'jeremiah', 'jeremiahyu050@gmail.com', 'eastwood', 'Out for Delivery', '2025-03-27 23:22:38', '2025-03-27 23:22:48'),
('3', 'si', 'ako', NULL, 'mars', 'In Transit', '2025-03-26 09:40:28', '2025-03-26 09:40:40'),
('33', 'syayan', 'carlo', 'carloyacial@gmail.com', 'samar', 'Out for Delivery', '2025-03-27 22:46:41', '2025-03-27 22:47:37'),
('333', 'xa', 'Ca', 'johncarloyacial6@gmail.com', 'sas', 'Out for Delivery', '2025-03-27 22:51:36', '2025-03-27 22:51:46'),
('4', 'si', 'sa', NULL, 'mars', 'In Transit', '2025-03-26 09:46:41', '2025-03-26 09:46:54'),
('5', 'mon', 'sai', NULL, 'mrong', 'Dispatched', '2025-03-27 20:51:17', NULL),
('6', 'sa', 'sa', NULL, 'sd', 'Dispatched', '2025-03-27 21:07:18', NULL),
('7', 'do', 're', NULL, 'mi', 'Dispatched', '2025-03-27 21:12:25', NULL),
('8', 'ako', 'si', NULL, '', 'Dispatched', '2025-03-27 21:29:54', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `package_personnel`
--

CREATE TABLE `package_personnel` (
  `tracking_number` varchar(50) NOT NULL,
  `personnel_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `package_personnel`
--

INSERT INTO `package_personnel` (`tracking_number`, `personnel_id`) VALUES
('1', 13);

-- --------------------------------------------------------

--
-- Table structure for table `personnel`
--

CREATE TABLE `personnel` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `personnel`
--

INSERT INTO `personnel` (`id`, `name`) VALUES
(13, 'saiman john');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `username`, `password`) VALUES
(1, 'ako', 'si'),
(2, 'sa', 'da'),
(3, 'jc', 'ya'),
(4, 'carlo', 'yacial');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `customers`
--
ALTER TABLE `customers`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `packages`
--
ALTER TABLE `packages`
  ADD PRIMARY KEY (`tracking_number`);

--
-- Indexes for table `package_personnel`
--
ALTER TABLE `package_personnel`
  ADD PRIMARY KEY (`tracking_number`,`personnel_id`),
  ADD KEY `personnel_id` (`personnel_id`);

--
-- Indexes for table `personnel`
--
ALTER TABLE `personnel`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `customers`
--
ALTER TABLE `customers`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `personnel`
--
ALTER TABLE `personnel`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `package_personnel`
--
ALTER TABLE `package_personnel`
  ADD CONSTRAINT `package_personnel_ibfk_1` FOREIGN KEY (`tracking_number`) REFERENCES `packages` (`tracking_number`),
  ADD CONSTRAINT `package_personnel_ibfk_2` FOREIGN KEY (`personnel_id`) REFERENCES `personnel` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
