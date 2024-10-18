using HealthcareHospitalManagementSystem.Infrastructure;
using HealthcareHospitalManagementSystem.Models;
using HealthcareHospitalManagementSystem.Services;

public class DoctorService : IDoctorService
{
    private List<Doctor> _doctors;
    private const int MaxDoctors = 50;
    public static int TotalDoctorsAdded { get; private set; } = 0;
    public Logger Logger { get; set; }

    public DoctorService()
    {
        _doctors = new List<Doctor>();
    }

    public void AddDoctor(Doctor doctor, NotificationService? notificationService)
    {
        string message;

        if (_doctors.Count >= MaxDoctors)
        {
            message = $"Cannot add doctor {doctor.Name}. Maximum number of doctors reached.";
            Logger?.LogError(message);
            throw new InvalidOperationException(message);
        }

        if (!ValidateDoctor(doctor))
        {
            message = $"Cannot add doctor {doctor.Name}. Invalid doctor details.";
            Logger?.LogError(message);
            throw new InvalidOperationException(message);
        }

        _doctors.Add(doctor);
        TotalDoctorsAdded++;
        message = $"Doctor {doctor.Name} added.";
        Logger?.Log(message);

        notificationService?.SendNotification($"Doctor {doctor.Name} has been successfully added.");
    }

    public List<Doctor> GetDoctors()
    {
        return _doctors;
    }

    public static bool ValidateDoctor(Doctor doctor)
    {
        return !string.IsNullOrWhiteSpace(doctor.Name) && !string.IsNullOrWhiteSpace(doctor.Specialization);
    }
}
