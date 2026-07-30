import { useDispatch, useSelector } from 'react-redux';
import LastActivity from '../components/last_activity/LastActivity';
import ProfileHeader from '../components/profile_header/ProfileHeader';
import ProfileProgress from '../components/profile_progress/ProfileProgress';
import ProfileSettings from '../components/profile_settings/ProfileSettings';
import Title from '../components/title/Title';
import styles from '../page/profile_page.module.css';
import { useEffect, useState } from 'react';
import { profileApi } from '../../../api/profile';
import { setUser } from '../../../redux/slices/authSlice';

function ProfilePage(){

    const dispatch = useDispatch();
    const user = useSelector(state => state.auth.user);

    const [profileData, setProfileData] = useState({});

    useEffect(() => {
        const fetchProfile = async() => {
            const response = await profileApi.getProfile();
            
            if(response.success){
                setProfileData(response.data);
                dispatch(setUser({
                    ...user,
                    avatarUrl: `${response.data?.avatarUrl}?t=${Date.now()}`
                }));
            }
            else{
                setProfileData({});
                toast.error(response.error);
            }
        }
        fetchProfile();
    }, []);

    return(
        <div className={styles.profile}>
            <ProfileHeader user={user}/>
            <ProfileProgress
                notesCount={profileData.notesCount}
                videosCount={profileData.videosCount}
                englishLevel={profileData.englishLevel}
            />
            <div className={styles.block}>
                <LastActivity activities={profileData?.activities}/>
                <ProfileSettings/>
            </div>
        </div>
    )
}

export default ProfilePage;