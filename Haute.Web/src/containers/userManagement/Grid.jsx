import React from 'react';
import { Card, Row } from 'reactstrap';
import ContextMenuContainer from 'containers/pages/ContextMenuContainer';
import Pagination from 'containers/pages/Pagination';
import GirdThumbListView from '../pages/userManagement/GirdThumbListView';
import { Colxx } from 'components/common/CustomBootstrap';

function collect(props) {
  return { data: props.data };
}

const Grid = ({
  items,
  allUserItems,
  displayMode,
  selectedItems,
  onCheckItem,
  currentPage,
  totalPage,
  onContextMenuClick,
  onContextMenu,
  onChangePage,
}) => {
  return (
    <Row>
      <Colxx xxs="12" className="">
        <Card>
          <div className="pl-2 d-flex flex-grow-1 min-width-zero">
            <div className="card-body align-self-center d-flex flex-column flex-lg-row justify-content-between min-width-zero align-items-lg-center">
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold text-start">
                First Name
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold">
                Last Name
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold">
                User Name
              </p>
              <p className="w-20 list-item-heading mb-1 truncate font-weight-bold ml-5">
                Email
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold">
                Phone
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold">
                Role Name
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold">
                Created Date
              </p>
              <p className="w-10 list-item-heading mb-1 truncate font-weight-bold">
                Is Active ?
              </p>
            </div>
          </div>
        </Card>
      </Colxx>
      {items.map((user) => {
        if (displayMode === 'thumblist') {
          return (
            <GirdThumbListView
              key={user.id}
              user={user}
              isSelect={selectedItems.includes(user.id)}
              collect={collect}
              onCheckItem={onCheckItem}
            />
          );
        }
        return <></>;
      })}
      {allUserItems === null ||
        (items.length === 0 && (
          <h3 className="mb-3 w-100 text-center">No Data Found</h3>
        ))}
      <Pagination
        currentPage={currentPage}
        totalPage={totalPage}
        onChangePage={(i) => onChangePage(i)}
      />
      <ContextMenuContainer
        onContextMenuClick={onContextMenuClick}
        onContextMenu={onContextMenu}
      />
    </Row>
  );
};

export default React.memo(Grid);
