import { ToastContainer } from 'react-toastify';
import Layout from './layout/Layout'
import HomePage from './pages/home/HomePage';
import { Navigate, Outlet, Route, Routes } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { useAuthMe } from './hooks/auth_me/useAuthMe';
import { useEffect, useState } from 'react';
import DictionaryPage from './pages/dictionary/DictionaryPage';
import ContentPage from './pages/content/ContentPage';

function App() {

  const {loading} = useAuthMe();
  const user = useSelector(state => state.auth.user);
  const isAuth = !!user;

  const [showLoader, setShowLoader] = useState(true);

  useEffect(() => {
    if(!loading){
      const timer = setTimeout(() => {
        setShowLoader(false);
      }, 400);

      return () => clearTimeout(timer);
    }
  }, [loading])

  if(showLoader || loading){
      return <div className="" style={{display: 'flex', justifyContent: 'center', color: 'white'}}><p>Идет загрузка...</p></div>
  }

  return (
    <>
      <Routes>
          <Route element={<Layout/>}>
            <Route path='/' element={<HomePage/>}/>
            <Route element={<ProtectedRoute isAuth={isAuth}/>}>
              <Route path='/videos/' element={<ContentPage/>}/>
              <Route path='/dictionary' element={<DictionaryPage/>}/>
            </Route>
          </Route>
      
          <Route path='*' element={<p style={{color: 'white'}}>Страница не найдена</p>}/>
      </Routes>

      <ToastContainer
        position='top-right'
        autoClose={2000}
        hideProgressBar={true}
        pauseOnFocusLoss={false}
        pauseOnHover={false}
        theme='dark'
      />
    </>
  )
}

export default App

function ProtectedRoute({isAuth}){
  
  if(!isAuth)
    return <Navigate to='/' replace/>

  return <Outlet/>
}