import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import "./Auth.css";

const Register = () => {
  const [nickname, setNickname] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const [nicknameError, setNicknameError] = useState("");
  const [emailError, setEmailError] = useState("");
  const [passwordError, setPasswordError] = useState("");
  const [confirmPasswordError, setConfirmPasswordError] = useState("");

  const [touched, setTouched] = useState({
    nickname: false,
    email: false,
    password: false,
    confirmPassword: false,
  });

  const { register } = useAuth();
  const navigate = useNavigate();

  const validateNickname = (value) => {
    if (!value) {
      return "Nickname is required";
    }
    if (value.length < 3) {
      return "Nickname must be at least 3 characters";
    }
    if (value.length > 30) {
      return "Nickname must be at most 30 characters";
    }
    const nicknameRegex = /^[a-zA-Z0-9_]+$/;
    if (!nicknameRegex.test(value)) {
      return "Nickname can only contain letters, numbers, and underscores";
    }
    return "";
  };

  const validateEmail = (value) => {
    if (!value) {
      return "Email is required";
    }
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(value)) {
      return "Please enter a valid email address";
    }
    return "";
  };

  const validatePassword = (value) => {
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
    if (value !== password) {
      return "Passwords do not match";
    }
    return "";
  };

  const handleNicknameChange = (e) => {
    const value = e.target.value;
    setNickname(value);
    if (touched.nickname) {
      setNicknameError(validateNickname(value));
    }
  };

  const handleEmailChange = (e) => {
    const value = e.target.value;
    setEmail(value);
    if (touched.email) {
      setEmailError(validateEmail(value));
    }
  };

  const handlePasswordChange = (e) => {
    const value = e.target.value;
    setPassword(value);
    if (touched.password) {
      setPasswordError(validatePassword(value));
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

  const handleBlur = (field) => {
    setTouched({ ...touched, [field]: true });

    switch (field) {
      case "nickname":
        setNicknameError(validateNickname(nickname));
        break;
      case "email":
        setEmailError(validateEmail(email));
        break;
      case "password":
        setPasswordError(validatePassword(password));
        break;
      case "confirmPassword":
        setConfirmPasswordError(validateConfirmPassword(confirmPassword));
        break;
      default:
        break;
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    // Mark all fields as touched
    setTouched({
      nickname: true,
      email: true,
      password: true,
      confirmPassword: true,
    });

    // Validate all fields
    const nicknameErr = validateNickname(nickname);
    const emailErr = validateEmail(email);
    const passwordErr = validatePassword(password);
    const confirmPasswordErr = validateConfirmPassword(confirmPassword);

    setNicknameError(nicknameErr);
    setEmailError(emailErr);
    setPasswordError(passwordErr);
    setConfirmPasswordError(confirmPasswordErr);

    // If any errors, don't submit
    if (nicknameErr || emailErr || passwordErr || confirmPasswordErr) {
      return;
    }

    setLoading(true);

    try {
      await register(nickname, email, password);
      navigate("/");
    } catch (err) {
      setError(
        err.response?.data?.message || "Failed to register. Please try again."
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-content">
        <div className="auth-box">
          <h1 className="auth-logo">MovieLibrary</h1>
          <h2 className="auth-title">Create account</h2>
          {error && <div className="auth-error">{error}</div>}
          <form onSubmit={handleSubmit} className="auth-form">
            <div className="form-group">
              <input
                type="text"
                placeholder="Nickname (letters, numbers, underscore)"
                value={nickname}
                onChange={handleNicknameChange}
                onBlur={() => handleBlur("nickname")}
                className={`auth-input ${
                  touched.nickname
                    ? nicknameError
                      ? "input-error"
                      : "input-success"
                    : ""
                }`}
              />
              {touched.nickname && nicknameError && (
                <div className="field-error">{nicknameError}</div>
              )}
            </div>
            <div className="form-group">
              <input
                type="email"
                placeholder="Email address"
                value={email}
                onChange={handleEmailChange}
                onBlur={() => handleBlur("email")}
                className={`auth-input ${
                  touched.email
                    ? emailError
                      ? "input-error"
                      : "input-success"
                    : ""
                }`}
              />
              {touched.email && emailError && (
                <div className="field-error">{emailError}</div>
              )}
            </div>
            <div className="form-group">
              <div className="password-group">
                <input
                  type={showPassword ? "text" : "password"}
                  placeholder="Password (min. 6 characters)"
                  value={password}
                  onChange={handlePasswordChange}
                  onBlur={() => handleBlur("password")}
                  className={`auth-input ${
                    touched.password
                      ? passwordError
                        ? "input-error"
                        : "input-success"
                      : ""
                  }`}
                />
                <button
                  type="button"
                  className="password-toggle"
                  onClick={() => setShowPassword(!showPassword)}
                  aria-label={showPassword ? "Hide password" : "Show password"}
                >
                  {showPassword ? (
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
              {touched.password && passwordError && (
                <div className="field-error">{passwordError}</div>
              )}
            </div>
            <div className="form-group">
              <div className="password-group">
                <input
                  type={showConfirmPassword ? "text" : "password"}
                  placeholder="Confirm password"
                  value={confirmPassword}
                  onChange={handleConfirmPasswordChange}
                  onBlur={() => handleBlur("confirmPassword")}
                  className={`auth-input ${
                    touched.confirmPassword
                      ? confirmPasswordError
                        ? "input-error"
                        : "input-success"
                      : ""
                  }`}
                />
                <button
                  type="button"
                  className="password-toggle"
                  onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                  aria-label={
                    showConfirmPassword ? "Hide password" : "Show password"
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
                <div className="field-error">{confirmPasswordError}</div>
              )}
            </div>
            <button type="submit" disabled={loading} className="auth-button">
              <span>{loading ? "Creating Account..." : "Sign In"}</span>
            </button>
          </form>
          <div className="auth-footer">
            <p>
              Already have an account?{" "}
              <Link to="/login" className="auth-link">
                Sign In
              </Link>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Register;
