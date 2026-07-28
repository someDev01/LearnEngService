import { NavLink } from 'react-router-dom';
import ButtonX from '../../ui/button_x/ButtonX';
import styles from '../side_bar/side_bar.module.css';
import { BookMarkedIcon, Headphones, HomeIcon, PlayCircleIcon, User } from 'lucide-react';
import { useEffect, useState } from 'react';
import SideBarItem from '../../ui/side_bar_item/SideBarItem';
import { useDispatch } from 'react-redux';
import { openModalTraining } from '../../redux/slices/modalSlice';

function SideBar({isOpen, onClose}){
    
    const [isOpening, setIsOpening] = useState(false);

    const dispatch = useDispatch();

    useEffect(() => {
        if(isOpen)
            requestAnimationFrame(() => setIsOpening(true));
        else setIsOpening(false);
    }, [isOpen]);

    const onOpenTraining = () => {
        dispatch(openModalTraining());
        onClose();
    };

    const items = [
        {
            to: "/",
            title: "Главная",
            Icon: HomeIcon
        },
        {
            to: "/dictionary",
            title: "Словарь",
            Icon: BookMarkedIcon
        },
        {
            to: "/videos",
            title: "Видео",
            Icon: PlayCircleIcon
        },
        {
            to:'',
            title: "Тренировка",
            Icon: Headphones,
            onAction: onOpenTraining
        },
        {
            to: "/profile",
            title: "Профиль",
            Icon: User
        }
    ];

    return(
        <div className={`${styles.overlay} ${isOpening ? styles.overlayOpen : ''}`} onClick={onClose}>
            <aside className={`${styles.side_bar} ${isOpening ? styles.open : ''}`}>
                <ButtonX onClick={onClose}/>
                <nav>
                    {items.map(item => (
                        <SideBarItem
                            key={item.to}
                            {...item}
                            onClick={onClose}
                        />
                    ))}
                </nav>
            </aside>
        </div>
    )
}

export default SideBar;