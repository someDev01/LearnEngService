import { useSelector } from 'react-redux';
import LastActivity from '../components/last_activity/LastActivity';
import ProfileHeader from '../components/profile_header/ProfileHeader';
import ProfileProgress from '../components/profile_progress/ProfileProgress';
import ProfileSettings from '../components/profile_settings/ProfileSettings';
import Title from '../components/title/Title';
import styles from '../page/profile_page.module.css';
import { useEffect, useState } from 'react';
import { profileApi } from '../../../api/profile';

function ProfilePage(){

    const user = useSelector(state => state.auth.user);

    const [profileData, setProfileData] = useState({});

    useEffect(() => {
        const fetchProfile = async() => {
            const response = await profileApi.getProfile();
            
            if(response.success){
                setProfileData(response.data);
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
                trainedCount={profileData.trainedCount}
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