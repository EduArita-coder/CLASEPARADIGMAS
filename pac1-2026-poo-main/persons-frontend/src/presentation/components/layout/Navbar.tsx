import { Home, Menu, Users, X } from "lucide-react";
import { useState } from "react";
import { MobileNavLink } from "./MobileNavLink";
import { NavLink } from "./NavLink";
import { useLocation } from "react-router";

export const Navbar = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  const location = useLocation();

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
    console.log({ isMenuOpen });
  };

  const isActive = (path: string) => {
    if (path === "/") {
      return location.pathname === "/";
    }

    return location.pathname.startsWith(path);
  };

  return (
    <nav className="bg-blue-600 text-white shadow-md">
      <div className="max-w-7xl mx-auto px-4">
        <div className="flex justify-between h-16">
          {/* Inicio Logo y Titulo */}
          <div className="flex items-center">
            <span className="font-bold text-xl">Persons</span>
          </div>
          {/* Fin Logo y Titulo */}
          {/* Inicio Navegación en escritorio */}
          <div className="hidden md:flex items-center space-x-4">
            <NavLink icon={<Home size={18} />} active={isActive('/')}  text="Inicio" to="/" />
            <NavLink icon={<Users size={18} />} active={isActive('/persons')} text="Personas" to="/persons" />
          </div>
          {/* Fin Navegación en escritorio */}
          {/* Inicio Botón menú móvil */}
          <div className="md:hidden flex items-center">
            <button
              onClick={toggleMenu}
              className="text-white hover:text-blue-200 focus:outline-none"
            >
              {isMenuOpen ? <X /> : <Menu />}
            </button>
          </div>
          {/* Fin Botón menú móvil */}
        </div>
      </div>
      {/* Inicio Menú Móvil */}
      {isMenuOpen && (
        <div className="md:hidden">
          <div className="px-2 pt-2 pb-3 space-y-1 sm:px-3">
            <MobileNavLink
              icon={<Home size={18} />}
              text="Inicio"
              to="/"
              active={isActive('/')}
            />
            <MobileNavLink
              icon={<Users size={18} />}
              text="Personas"
              to="/persons"
              active={isActive('/persons')}
            />
          </div>
        </div>
      )}
      {/* Fin Menú Móvil */}
    </nav>
  );
};
