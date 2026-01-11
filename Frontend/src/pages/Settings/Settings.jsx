import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import api from "../../services/api";
import Header from "../../components/Header";
import "./Settings.css";

const Settings = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const [nickname, setNickname] = useState(user?.nickname || "");
  const [email, setEmail] = useState(user?.email || "");
  const [profilePictureUrl, setProfilePictureUrl] = useState(
    user?.profilePictureUrl || ""
  );
  const [selectedFile, setSelectedFile] = useState(null);
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState({ type: "", text: "" });
  const [activeSection, setActiveSection] = useState("account");
  const [showCurrentPassword, setShowCurrentPassword] = useState(false);
  const [showNewPassword, setShowNewPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [newPasswordError, setNewPasswordError] = useState("");
  const [confirmPasswordError, setConfirmPasswordError] = useState("");
  const [touched, setTouched] = useState({
    newPassword: false,
    confirmPassword: false,
  });

  const showMessage = (type, text) => {
    setMessage({ type, text });
    setTimeout(() => setMessage({ type: "", text: "" }), 5000);
  };

  // Validation functions
  const validateNewPassword = (value) => {
    if (!value) {
      return "Password is required";
    }
    if (value.length < 6) {
      return "Password must be at least 6 characters";
    }
    return "";
  };

  const validateConfirmPassword = (value) => {
    if (!value) {
      return "Please confirm your password";
    }
    if (value !== newPassword) {
      return "Passwords do not match";
    }
    return "";
  };

  const handleNewPasswordChange = (e) => {
    const value = e.target.value;
    setNewPassword(value);
    if (touched.newPassword) {
      setNewPasswordError(validateNewPassword(value));
    }
    if (touched.confirmPassword && confirmPassword) {
      setConfirmPasswordError(
        value !== confirmPassword ? "Passwords do not match" : ""
      );
    }
  };

  const handleConfirmPasswordChange = (e) => {
    const value = e.target.value;
    setConfirmPassword(value);
    if (touched.confirmPassword) {
      setConfirmPasswordError(validateConfirmPassword(value));
    }
  };

  const handlePasswordBlur = (field) => {
    setTouched({ ...touched, [field]: true });

    if (field === "newPassword") {
      setNewPasswordError(validateNewPassword(newPassword));
    } else if (field === "confirmPassword") {
      setConfirmPasswordError(validateConfirmPassword(confirmPassword));
    }
  };

  const getImageUrl = (url) => {
    if (!url) return null;
    if (url.startsWith("http")) return url;
    const baseUrl = import.meta.env.VITE_API_URL
      ? import.meta.env.VITE_API_URL.replace("/api", "")
      : "http://localhost:5002";
    return `${baseUrl}${url}`;
  };

  const handleUpdateProfile = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      let updatedUserResponse = null;

      if (selectedFile) {
        const formData = new FormData();
        formData.append("file", selectedFile);
        const uploadResponse = await api.post(
          `/users/${user.userId}/profile-picture`,
          formData,
          {
            headers: {
              "Content-Type": "multipart/form-data",
            },
          }
        );
        updatedUserResponse = uploadResponse.data.data;
        setProfilePictureUrl(updatedUserResponse.profilePictureUrl);
      }

      const updates = {};
      if (nickname !== user.nickname) updates.nickname = nickname;
      if (email !== user.email) updates.email = email;
      if (!selectedFile && profilePictureUrl !== user.profilePictureUrl)
        updates.profilePictureUrl = profilePictureUrl;

      if (Object.keys(updates).length > 0) {
        const response = await api.put(`/users/${user.userId}`, updates);
        updatedUserResponse = response.data.data;
      }

      if (!selectedFile && Object.keys(updates).length === 0) {
        setLoading(false);
        return;
      }

      if (updatedUserResponse) {
        const currentUser = JSON.parse(localStorage.getItem("user"));
        const newUserData = { ...currentUser, ...updatedUserResponse };
        localStorage.setItem("user", JSON.stringify(newUserData));
      }

      showMessage("success", "Profile updated successfully!");

      setTimeout(() => window.location.reload(), 1000);
    } catch (err) {
      showMessage(
        "error",
        err.response?.data?.message || "Failed to update profile"
      );
    } finally {
      setLoading(false);
    }
  };

  const handleChangePassword = async (e) => {
    e.preventDefault();

    setTouched({
      newPassword: true,
      confirmPassword: true,
    });

    const newPasswordErr = validateNewPassword(newPassword);
    const confirmPasswordErr = validateConfirmPassword(confirmPassword);

    setNewPasswordError(newPasswordErr);
    setConfirmPasswordError(confirmPasswordErr);

    if (newPasswordErr || confirmPasswordErr) {
      return;
    }

    setLoading(true);

    try {
      await api.put(`/users/${user.userId}`, {
        password: newPassword,
      });

      setCurrentPassword("");
      setNewPassword("");
      setConfirmPassword("");
      setNewPasswordError("");
      setConfirmPasswordError("");
      setTouched({ newPassword: false, confirmPassword: false });
      showMessage("success", "Password changed successfully!");
    } catch (err) {
      showMessage(
        "error",
        err.response?.data?.message || "Failed to change password"
      );
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteAccount = async () => {
    const confirmed = window.confirm(
      "Are you sure you want to delete your account? This action cannot be undone."
    );

    if (!confirmed) return;

    const doubleConfirm = window.confirm(
      "This will permanently delete all your data. Are you absolutely sure?"
    );

    if (!doubleConfirm) return;

    setLoading(true);

    try {
      await api.delete(`/users/${user.userId}`);
      logout();
      navigate("/register");
      showMessage("success", "Account deleted successfully");
    } catch (err) {
      showMessage(
        "error",
        err.response?.data?.message || "Failed to delete account"
      );
      setLoading(false);
    }
  };

  return (
    <>
      <Header />
      <main className="settings-page">
        <div className="settings-container">
          <h1 className="settings-title">Account Settings</h1>

          {message.text && (
            <div className={`message ${message.type}`}>{message.text}</div>
          )}

          <div className="settings-layout">
            {/* Sidebar Navigation */}
            <nav className="settings-nav">
              <button
                className={`nav-item ${
                  activeSection === "account" ? "active" : ""
                }`}
                onClick={() => setActiveSection("account")}
              >
                <svg
                  width="20"
                  height="20"
                  viewBox="0 0 20 20"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M10 10C12.7614 10 15 7.76142 15 5C15 2.23858 12.7614 0 10 0C7.23858 0 5 2.23858 5 5C5 7.76142 7.23858 10 10 10Z"
                    stroke="currentColor"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                  <path
                    d="M17.07 20C17.07 16.13 13.9 13 10 13C6.1 13 2.93 16.13 2.93 20"
                    stroke="currentColor"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                </svg>
                Account Info
              </button>
              <button
                className={`nav-item ${
                  activeSection === "security" ? "active" : ""
                }`}
                onClick={() => setActiveSection("security")}
              >
                <svg
                  width="20"
                  height="20"
                  viewBox="0 0 20 20"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M15 8.33333V5C15 2.23858 12.7614 0 10 0C7.23858 0 5 2.23858 5 5V8.33333"
                    stroke="currentColor"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                  <path
                    d="M4.16667 8.33333H15.8333C17.214 8.33333 18.3333 9.45262 18.3333 10.8333V16.6667C18.3333 18.0474 17.214 19.1667 15.8333 19.1667H4.16667C2.78595 19.1667 1.66667 18.0474 1.66667 16.6667V10.8333C1.66667 9.45262 2.78595 8.33333 4.16667 8.33333Z"
                    stroke="currentColor"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                </svg>
                Security
              </button>
              <button
                className={`nav-item ${
                  activeSection === "danger" ? "active" : ""
                }`}
                onClick={() => setActiveSection("danger")}
              >
                <svg
                  width="20"
                  height="20"
                  viewBox="0 0 20 20"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M10 7.5V10.8333"
                    stroke="currentColor"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                  <path
                    d="M10 14.1667H10.0083"
                    stroke="currentColor"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                  <path
                    d="M8.57499 2.49167L1.51666 14.1667C1.37037 14.4187 1.29322 14.7053 1.29266 14.997C1.29211 15.2887 1.36818 15.5756 1.51353 15.8281C1.65889 16.0807 1.86843 16.2902 2.12098 16.4356C2.37354 16.5809 2.66043 16.6571 2.95249 16.6567H17.0692C17.3612 16.6571 17.6481 16.5809 17.9007 16.4356C18.1532 16.2902 18.3628 16.0807 18.5081 15.8281C18.6535 15.5756 18.7296 15.2887 18.729 14.997C18.7284 14.7053 18.6513 14.4187 18.505 14.1667L11.4467 2.49167C11.3002 2.24267 11.0912 2.03654 10.8402 1.89367C10.5892 1.7508 10.3048 1.6759 10.0158 1.6759C9.72686 1.6759 9.44243 1.7508 9.19145 1.89367C8.94046 2.03654 8.73145 2.24267 8.58499 2.49167V2.49167Z"
                    stroke="currentColor"
                    strokeWidth="1.5"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                  />
                </svg>
                Danger Zone
              </button>
            </nav>

            {/* Content Area */}
            <div className="settings-content">
              {/* Account Info Section */}
              {activeSection === "account" && (
                <div className="settings-section">
                  <h2>Account Information</h2>
                  <p className="section-description">
                    Update your account details and personal information
                  </p>

                  <form
                    onSubmit={handleUpdateProfile}
                    className="settings-form"
                  >
                    <div className="account-layout">
                      <div className="account-fields">
                        <div className="form-group">
                          <label htmlFor="nickname">Nickname</label>
                          <input
                            type="text"
                            id="nickname"
                            value={nickname}
                            onChange={(e) => setNickname(e.target.value)}
                            required
                            className="form-input"
                          />
                        </div>

                        <div className="form-group">
                          <label htmlFor="email">Email Address</label>
                          <input
                            type="email"
                            id="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                            className="form-input"
                          />
                        </div>

                        <div className="form-group">
                          <label>Account Role</label>
                          <input
                            type="text"
                            value={user?.role || "User"}
                            disabled
                            className="form-input disabled"
                          />
                          <small className="form-hint">
                            Your account role cannot be changed
                          </small>
                        </div>
                      </div>

                      <div className="account-picture">
                        <div className="profile-picture-container">
                          <div className="profile-preview">
                            <img
                              src={
                                selectedFile
                                  ? URL.createObjectURL(selectedFile)
                                  : getImageUrl(profilePictureUrl) ||
                                    `https://api.dicebear.com/7.x/avataaars/svg?seed=${nickname}`
                              }
                              alt="Profile preview"
                              onError={(e) => {
                                e.target.src = `https://api.dicebear.com/7.x/avataaars/svg?seed=${nickname}`;
                              }}
                            />
                          </div>
                          <input
                            type="file"
                            id="profilePictureFile"
                            accept="image/*"
                            onChange={(e) => {
                              if (e.target.files[0]) {
                                setSelectedFile(e.target.files[0]);
                              }
                            }}
                            style={{ display: "none" }}
                          />
                          <button
                            type="button"
                            className="btn-edit-picture"
                            onClick={() =>
                              document
                                .getElementById("profilePictureFile")
                                .click()
                            }
                          >
                            <svg
                              width="16"
                              height="16"
                              viewBox="0 0 24 24"
                              fill="none"
                              stroke="currentColor"
                              strokeWidth="2"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                            >
                              <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                              <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                            </svg>
                            Edit Photo
                          </button>
                          {selectedFile && (
                            <small className="form-hint selected-file">
                              Selected: {selectedFile.name}
                            </small>
                          )}
                        </div>
                      </div>
                    </div>

                    <button
                      type="submit"
                      disabled={loading}
                      className="btn btn-primary"
                    >
                      {loading ? "Saving..." : "Save Changes"}
                    </button>
                  </form>
                </div>
              )}

              {/* Security Section */}
              {activeSection === "security" && (
                <div className="settings-section">
                  <h2>Security Settings</h2>
                  <p className="section-description">
                    Manage your password and account security
                  </p>

                  <form
                    onSubmit={handleChangePassword}
                    className="settings-form"
                  >
                    <div className="form-group">
                      <label htmlFor="current-password">Current Password</label>
                      <div className="password-group">
                        <input
                          type={showCurrentPassword ? "text" : "password"}
                          id="current-password"
                          value={currentPassword}
                          onChange={(e) => setCurrentPassword(e.target.value)}
                          className="form-input"
                          placeholder="Enter current password"
                        />
                        <button
                          type="button"
                          className="password-toggle"
                          onClick={() =>
                            setShowCurrentPassword(!showCurrentPassword)
                          }
                          aria-label={
                            showCurrentPassword
                              ? "Hide password"
                              : "Show password"
                          }
                        >
                          {showCurrentPassword ? (
                            <svg
                              width="20"
                              height="20"
                              viewBox="0 0 24 24"
                              fill="none"
                              stroke="currentColor"
                              strokeWidth="2"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                            >
                              <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path>
                              <circle cx="12" cy="12" r="3"></circle>
                            </svg>
                          ) : (
                            <svg
                              width="20"
                              height="20"
                              viewBox="0 0 24 24"
                              fill="none"
                              stroke="currentColor"
                              strokeWidth="2"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                            >
                              <path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"></path>
                              <line x1="1" y1="1" x2="23" y2="23"></line>
                            </svg>
                          )}
                        </button>
                      </div>
                    </div>

                    <div className="form-group">
                      <label htmlFor="new-password">New Password</label>
                      <div className="password-group">
                        <input
                          type={showNewPassword ? "text" : "password"}
                          id="new-password"
                          value={newPassword}
                          onChange={handleNewPasswordChange}
                          onBlur={() => handlePasswordBlur("newPassword")}
                          className={`form-input ${
                            touched.newPassword
                              ? newPasswordError
                                ? "input-error"
                                : "input-success"
                              : ""
                          }`}
                          placeholder="Enter new password"
                        />
                        <button
                          type="button"
                          className="password-toggle"
                          onClick={() => setShowNewPassword(!showNewPassword)}
                          aria-label={
                            showNewPassword ? "Hide password" : "Show password"
                          }
                        >
                          {showNewPassword ? (
                            <svg
                              width="20"
                              height="20"
                              viewBox="0 0 24 24"
                              fill="none"
                              stroke="currentColor"
                              strokeWidth="2"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                            >
                              <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path>
                              <circle cx="12" cy="12" r="3"></circle>
                            </svg>
                          ) : (
                            <svg
                              width="20"
                              height="20"
                              viewBox="0 0 24 24"
                              fill="none"
                              stroke="currentColor"
                              strokeWidth="2"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                            >
                              <path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"></path>
                              <line x1="1" y1="1" x2="23" y2="23"></line>
                            </svg>
                          )}
                        </button>
                      </div>
                      {touched.newPassword && newPasswordError && (
                        <div className="field-error">{newPasswordError}</div>
                      )}
                      {!newPasswordError && (
                        <small className="form-hint">
                          Must be at least 6 characters
                        </small>
                      )}
                    </div>

                    <div className="form-group">
                      <label htmlFor="confirm-password">
                        Confirm New Password
                      </label>
                      <div className="password-group">
                        <input
                          type={showConfirmPassword ? "text" : "password"}
                          id="confirm-password"
                          value={confirmPassword}
                          onChange={handleConfirmPasswordChange}
                          onBlur={() => handlePasswordBlur("confirmPassword")}
                          className={`form-input ${
                            touched.confirmPassword
                              ? confirmPasswordError
                                ? "input-error"
                                : "input-success"
                              : ""
                          }`}
                          placeholder="Confirm new password"
                        />
                        <button
                          type="button"
                          className="password-toggle"
                          onClick={() =>
                            setShowConfirmPassword(!showConfirmPassword)
                          }
                          aria-label={
                            showConfirmPassword
                              ? "Hide password"
                              : "Show password"
                          }
                        >
                          {showConfirmPassword ? (
                            <svg
                              width="20"
                              height="20"
                              viewBox="0 0 24 24"
                              fill="none"
                              stroke="currentColor"
                              strokeWidth="2"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                            >
                              <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path>
                              <circle cx="12" cy="12" r="3"></circle>
                            </svg>
                          ) : (
                            <svg
                              width="20"
                              height="20"
                              viewBox="0 0 24 24"
                              fill="none"
                              stroke="currentColor"
                              strokeWidth="2"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                            >
                              <path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"></path>
                              <line x1="1" y1="1" x2="23" y2="23"></line>
                            </svg>
                          )}
                        </button>
                      </div>
                      {touched.confirmPassword && confirmPasswordError && (
                        <div className="field-error">
                          {confirmPasswordError}
                        </div>
                      )}
                    </div>

                    <button
                      type="submit"
                      disabled={loading}
                      className="btn btn-primary"
                    >
                      {loading ? "Changing..." : "Change Password"}
                    </button>
                  </form>
                </div>
              )}

              {/* Danger Zone Section */}
              {activeSection === "danger" && (
                <div className="settings-section danger-zone">
                  <h2>Danger Zone</h2>
                  <p className="section-description">
                    Irreversible and destructive actions
                  </p>

                  <div className="danger-card">
                    <div className="danger-info">
                      <h3>Delete Account</h3>
                      <p>
                        Once you delete your account, there is no going back.
                        All your data, including your watchlist and ratings,
                        will be permanently deleted.
                      </p>
                    </div>
                    <button
                      onClick={handleDeleteAccount}
                      disabled={loading}
                      className="btn btn-danger"
                    >
                      Delete Account
                    </button>
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>
      </main>
    </>
  );
};

export default Settings;
