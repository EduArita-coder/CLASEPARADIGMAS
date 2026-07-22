export const Loader = () => { 
    return (
        <div className="fixed top-0 left-0 bottom-0 transition-opacity opacity-30">
            <div className="flex items-center justify-center min-h-screen p-5 bg-gray-100 min-w-screen">
                <div className="flex space-x-2 animate-pulse">
                    <div className="w-3 h-3 bg-gray-500 rounded-full"></div>
                    <div className="w-3 h-3 bg-gray-500 rounded-full"></div>
                    <div className="w-3 h-3 bg-gray-500 rounded-full"></div>
                </div>
            </div>
        </div>
    )
}